using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera newCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Camera.main.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(true);
        }
    }
}
