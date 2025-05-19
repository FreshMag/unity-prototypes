using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MousePlatform : MonoBehaviour
{
    public bool canMove = true;

    [Header("Assign one active path")]
    public Path[] paths;

    private Path activePath;


    void Update()
    {
        if (!canMove) return;

        // Get active path
        activePath = null;
        foreach (var path in paths)
        {
            if (path != null && path.gameObject.activeInHierarchy)
            {
                activePath = path;
                break;
            }
        }
        if (activePath == null) return;

        // Project mouse to world using a plane
        Plane groundPlane = new Plane(Vector3.forward, transform.position); // or platform normal
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            // Get closest point on the path
            Vector3 targetPos = activePath.GetClosestPoint(hitPoint);

            transform.position = targetPos;
        }
    }
}
