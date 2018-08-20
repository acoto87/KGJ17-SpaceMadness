using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    public GameObject[] EnemiesPrefab;

    public PlayerController player;
    public new GameObject camera;

    public int enemyCount = 40;

    public float speed;
    private Vector3 _dir;

    public Text finishText;

    void Start()
    {
        for (int i = 0; i <= 80; i++)
        {
            for (int j = 0; j <= 80; j++)
            {
                var background = GameObject.Instantiate(BackgroundPrefab);
                background.transform.position = new Vector3(-20 + j * 2, -20 + i * 2, 0.0f);
                background.transform.parent = transform;
            }
        }

        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = GameObject.Instantiate(EnemiesPrefab[Random.Range(0, EnemiesPrefab.Length)]);
            enemy.transform.position = new Vector3(
                Random.Range(10.0f, 90.0f),
                Random.Range(10.0f, 90.0f),
                0.0f);
        }

        var randomPoint = new Vector3(
            Random.Range(10.0f, 90.0f),
            Random.Range(30.0f, 90.0f),
            -10.0f);

        _dir = Vector3.Normalize(randomPoint - transform.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (player == null)
        {
            finishText.text = "YOU LOST :(";
            finishText.color = Color.gray;
            return;
        }

        if (enemyCount == 0)
        {
            finishText.text = "YOU WON :)";
            finishText.color = new Color(240.0f / 255.0f, 240.0f / 255.0f, 240.0f / 255.0f, 1.0f);
            return;
        }

        var cameraPos = camera.transform.position;
        var playerPos = player.transform.position;

        if (cameraPos.x < 10)
        {
            player.ChangeDir(ShipDir.Right);

            _dir = Vector3.Reflect(_dir, Vector3.right).normalized;
        }
        else if (cameraPos.x > 90)
        {
            player.ChangeDir(ShipDir.Left);
            _dir = Vector3.Reflect(_dir, Vector3.left).normalized;
        }

        if (cameraPos.y < 10)
        {
            player.ChangeDir(ShipDir.Up);
            _dir = Vector3.Reflect(_dir, Vector3.up).normalized;
        }
        else if (cameraPos.y > 90)
        {
            player.ChangeDir(ShipDir.Down);
            _dir = Vector3.Reflect(_dir, Vector3.down).normalized;
        }

        cameraPos += _dir * speed * Time.deltaTime;
        playerPos += _dir * speed * Time.deltaTime;

        camera.transform.position = cameraPos;
        player.transform.position = playerPos;
    }
}
