using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Sprite[] sprites;
    public float speed;

    private Vector3 _goToPosition;
    private SpriteRenderer _renderer;
    private SpriteRenderer[] _allRenderers;
    private Animator _anim;
    private AudioSource _audio;

    private BackgroundController _background;

    private bool _destroyed;

    void Start()
    {
        _background = GameObject.FindObjectOfType<BackgroundController>();

        _anim = GetComponent<Animator>();

        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = sprites[Random.Range(0, sprites.Length)];

        _allRenderers = GetComponentsInChildren<SpriteRenderer>();

        _audio = GetComponent<AudioSource>();

        speed = Random.Range(3.0f, 8.0f);

        _goToPosition = new Vector3(
            Random.Range(10.0f, 90.0f),
            Random.Range(10.0f, 90.0f),
            0.0f);
    }

    void Update()
    {
        var position = transform.position;
        var distance = Vector3.Distance(position, _goToPosition);
        while (distance < 0.05f)
        {
            _goToPosition = new Vector3(
                Random.Range(10.0f, 90.0f),
                Random.Range(10.0f, 90.0f),
                0.0f);

            distance = Vector3.Distance(position, _goToPosition);
        }

        var dir = Vector3.Normalize(_goToPosition - position);

        transform.rotation = Quaternion.FromToRotation(Vector3.down, dir);

        position += dir * speed * Time.deltaTime;
        transform.position = position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_destroyed)
        {
            return;
        }

        var camera = Camera.main;
        var cameraPos = camera.transform.position;
        var height = 2.0f * camera.orthographicSize;
        var width = height * camera.aspect;

        var position = transform.position;
        if (position.x >= cameraPos.x - width * 0.5f &&
            position.x <= cameraPos.x + width * 0.5f &&
            position.y >= cameraPos.y - height * 0.5f &&
            position.y <= cameraPos.y + height * 0.5f)
        {
            if (other.CompareTag("Laser") || other.CompareTag("Player"))
            {
                if (_allRenderers != null)
                {
                    for (int i = 0; i < _allRenderers.Length - 1; i++)
                    {
                        _allRenderers[i].enabled = false;
                    }
                }

                _anim.SetBool("explode", true);
                _audio.Play();

                _background.enemyCount--;
                _destroyed = true;
                GameObject.Destroy(gameObject, 0.4f);
            }
        }
    }
}
