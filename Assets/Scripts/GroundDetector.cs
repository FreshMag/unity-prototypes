using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Detectors")]
    public SingleDetector upperLeft;
    public SingleDetector lowerLeft;
    public SingleDetector upperRight;
    public SingleDetector lowerRight;

    [Header("Indicators")]
    public GameObject leftIndicator;
    public GameObject rightIndicator;

    [Header("Platform Reference")]
    public Transform platform;

    public float rotationAngle = 90f;

    void Update()
    {
        bool canRotateLeft = !lowerLeft.hasGround;
        bool canRotateRight = !lowerRight.hasGround;

        leftIndicator.SetActive(canRotateLeft);
        rightIndicator.SetActive(canRotateRight);

        // Tasto sinistro ruota a sinistra
        if (Input.GetMouseButtonDown(0) && canRotateLeft)
        {
            RotatePlatform(-rotationAngle);
        }

        // Tasto destro ruota a destra
        if (Input.GetMouseButtonDown(1) && canRotateRight)
        {
            RotatePlatform(rotationAngle);
        }
    }

    void RotatePlatform(float angle)
    {
        platform.Rotate(0, 0, angle);
    }
}