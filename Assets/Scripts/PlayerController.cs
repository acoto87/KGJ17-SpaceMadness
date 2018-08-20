using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public ShipDir dir;
    public float speedMul = 1.2f;

    public float shootTimeDelay = 0.15f;
    private float lastShootTime;
    private int nextShootSide;

    public SpriteRenderer FireLeft, FireRight;
    public LaserController LaserPrefab;

    public AudioClip shootAudio1, shootAudio2, flipAudio, explosionAudio;

    private AudioSource _audio;
    private SpriteRenderer[] _allRenderers;
    private Animator _anim;

    public Sprite[] sprites;
    public int lives;
    public Image[] livesImages;
    public Text pointsText;

    private BackgroundController _background;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();

        _allRenderers = GetComponentsInChildren<SpriteRenderer>();

        _allRenderers[0].sprite = sprites[Random.Range(0, sprites.Length)];

        _background = GameObject.FindObjectOfType<BackgroundController>();
        nextShootSide = -1;
    }

    void Update()
    {
        var vdir = Vector3.zero;

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - lastShootTime > shootTimeDelay)
            {
                var laser = GameObject.Instantiate(LaserPrefab);
                laser.transform.position = transform.position + new Vector3(nextShootSide * 0.25f, 0.0f, 0.0f);

                switch (dir)
                {
                    case ShipDir.Up:
                    {
                        laser.dirx = 0.0f;
                        laser.diry = 1.0f;
                        laser.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
                        break;
                    }
                    case ShipDir.Right:
                    {
                        laser.dirx = 1.0f;
                        laser.diry = 0.0f;
                        laser.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.right);
                        break;
                    }
                    case ShipDir.Down:
                    {
                        laser.dirx = 0.0f;
                        laser.diry = -1.0f;
                        laser.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
                        break;
                    }
                    case ShipDir.Left:
                    {
                        laser.dirx = -1.0f;
                        laser.diry = 0.0f;
                        laser.transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.left);
                        break;
                    }
                }

                if (_audio.isPlaying)
                {
                    _audio.Stop();
                }

                if (nextShootSide < 0)
                {
                    _audio.clip = shootAudio1;
                }
                else
                {
                    _audio.clip = shootAudio2;
                }

                _audio.Play();

                nextShootSide = -nextShootSide;
                lastShootTime = Time.time;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            vdir.y = 1 * speedMul;
            FireLeft.enabled = true;
            FireRight.enabled = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            vdir.y = -1 * speedMul;
            FireLeft.enabled = true;
            FireRight.enabled = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            vdir.x = -1 * speedMul;
            FireLeft.enabled = true;
            FireRight.enabled = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            vdir.x = 1 * speedMul;
            FireLeft.enabled = true;
            FireRight.enabled = true;
        }

        if (vdir != Vector3.zero)
        {
            var camera = Camera.main;
            var cameraPos = camera.transform.position;
            var height = 2.0f * camera.orthographicSize;
            var width = height * camera.aspect;

            var position = transform.position;
            var newPosition = position + vdir * speed * Time.deltaTime; ;

            if (newPosition.x < cameraPos.x - width * 0.5f + 1.0f ||
                newPosition.x > cameraPos.x + width * 0.5f - 1.0f)
            {
                newPosition.x = position.x;
            }

            if (newPosition.y < cameraPos.y - height * 0.5f + 1.0f ||
                newPosition.y > cameraPos.y + height * 0.5f - 1.0f)
            {
                newPosition.y = position.y;
            }

            transform.position = newPosition;
        }

        pointsText.text = _background.enemyCount.ToString();
    }

    public void ChangeDir(ShipDir newDir)
    {
        dir = newDir;

        switch (dir)
        {
            case ShipDir.Up:
            {
                transform.rotation = Quaternion.identity;
                break;
            }
            case ShipDir.Right:
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                break;
            }
            case ShipDir.Down:
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                break;
            }
            case ShipDir.Left:
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
            }
        }

        var camera = Camera.main;
        var cameraPos = camera.transform.position;

        var position = transform.position;
        position.x = cameraPos.x - (position.x - cameraPos.x);
        position.y = cameraPos.y - (position.y - cameraPos.y);
        transform.position = position;

        if (_audio.isPlaying)
        {
            _audio.Stop();
        }

        _audio.clip = flipAudio;
        _audio.Play();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (lives == 0)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            livesImages[lives - 1].enabled = false;
            lives--;

            if (lives == 0)
            {
                for (int i = 0; i < _allRenderers.Length - 1; i++)
                {
                    _allRenderers[i].enabled = false;
                }

                _anim.SetBool("explode", true);
                GameObject.Destroy(gameObject, 0.4f);
            }

            if (_audio.isPlaying)
            {
                _audio.Stop();
            }

            _audio.clip = explosionAudio;
            _audio.Play();
        }
    }
}

public enum ShipDir
{
    Up,
    Right,
    Down,
    Left
}

