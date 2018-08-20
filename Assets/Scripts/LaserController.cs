using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float speed;
    public float dirx, diry;

    void Update()
    {
        transform.position += new Vector3(dirx, diry, 0.0f) * speed * Time.deltaTime;

        if (transform.position.x < 0 || transform.position.x > 100 ||
            transform.position.y < 0 || transform.position.y > 100)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
