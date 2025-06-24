using UnityEngine;

[RequireComponent(typeof(MousePlatform))]
public class BallCamera : MonoBehaviour
{
    public GameObject ballCamera;
    private GameObject oldCamera;

    [HideInInspector]
    public MousePlatform mousePlatform;
    private bool isBallCameraActive = false;

    void Start()
    {
        mousePlatform = GetComponent<MousePlatform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            isBallCameraActive = !isBallCameraActive;
            if (isBallCameraActive)
            {
                oldCamera = Camera.main.gameObject;
                ballCamera.SetActive(true);
                oldCamera.SetActive(false);
            }
            else
            {
                ballCamera.SetActive(false);
                oldCamera.SetActive(true);
            }
            
            mousePlatform.enabled = !isBallCameraActive;
        }
    }
}
