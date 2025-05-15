using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MousePlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float minX = -8f; // Left limit
    public float maxX = 8f;  // Right limit
    public float yPosition = -4f; // Constant Y position
    public float zPosition = 0f;  // Constant Z position

    public Camera mainCamera;
    public bool canMove = true;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!canMove) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 localMousePos = transform.InverseTransformPoint(mouseWorldPos);

        // Only move along the local X axis
        localMousePos.y = 0;

        // Convert back to world space
        Vector3 targetWorldPos = transform.TransformPoint(localMousePos);

        // Apply the new position, keeping the platform's local Y/Z unchanged
        transform.position = new Vector3(
            targetWorldPos.x,
            targetWorldPos.y
        );
    }
}
