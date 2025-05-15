using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera newCamera;
    private Camera currentCamera;
    public float rotationAngle = 90f;

    void Start()
    {
        if (currentCamera == null)
        {
            currentCamera = Camera.main;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentCamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(true);
        }
    }
}
