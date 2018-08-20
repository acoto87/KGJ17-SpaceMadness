using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject rect;

    void Update()
    {
        var camera = GetComponent<Camera>();
        var height = 2.0f * camera.orthographicSize;
        var width = height * camera.aspect;

        rect.transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        rect.transform.localScale = new Vector3(width, height, 1.0f);
    }
}
