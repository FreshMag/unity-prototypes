using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera newCamera;
    public float transitionDuration = 1.0f;

    private float t = 0f;
    private bool isTransitioning = false;
    private Vector3 newPosition;
    private Quaternion newRotation;
    private float newFov;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && newCamera != Camera.main)
        {
            StartTransition(Camera.main);
        }
    }

    void Update()
    {
        if (isTransitioning)
        {
            t += Time.deltaTime / transitionDuration;
            newCamera.transform.position = Vector3.Lerp(newCamera.transform.position, newPosition, t);
            newCamera.transform.rotation = Quaternion.Lerp(newCamera.transform.rotation, newRotation, t);
            newCamera.fieldOfView = Mathf.Lerp(newCamera.fieldOfView, newFov, t);

            if (t >= 1f)
            {
                isTransitioning = false;
            }
        }
    }

    public void StartTransition(Camera oldCamera)
    {
        newPosition = newCamera.transform.position;
        newRotation = newCamera.transform.rotation;
        newFov = newCamera.fieldOfView;

        newCamera.transform.position = oldCamera.transform.position;
        newCamera.transform.rotation = oldCamera.transform.rotation;
        newCamera.fieldOfView = oldCamera.fieldOfView;
        t = 0f;
        isTransitioning = true;
        newCamera.gameObject.SetActive(true);
        oldCamera.gameObject.SetActive(false);
    }
}
