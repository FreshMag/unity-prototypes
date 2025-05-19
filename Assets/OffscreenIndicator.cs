using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicator : MonoBehaviour
{
    public Transform target;           // The object to track
    private Camera mainCamera;
    public RectTransform pointerUI;    // The UI arrow image
    public float screenEdgeBuffer = 50f; // How far from edge to clamp

    private TextMeshProUGUI text;

  void Start()
  {
    text = pointerUI.transform.GetComponentInChildren<TextMeshProUGUI>();
  }

  void Update()
    {
        mainCamera = Camera.main;
        if (target == null || mainCamera == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        bool isOffscreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffscreen)
        {
            pointerUI.gameObject.SetActive(true);

            text.SetText(Vector3.Distance(target.position, mainCamera.transform.position).ToString());

            // Direction from screen center to target
            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
            Vector3 dir = (screenPos - screenCenter).normalized;

            // Position on edge of screen
            Vector3 edgePosition = screenCenter + dir * ((Mathf.Min(Screen.width, Screen.height) / 2f) - screenEdgeBuffer);
            edgePosition.x = Mathf.Clamp(edgePosition.x, screenEdgeBuffer, Screen.width - screenEdgeBuffer);
            edgePosition.y = Mathf.Clamp(edgePosition.y, screenEdgeBuffer, Screen.height - screenEdgeBuffer);

            pointerUI.position = edgePosition;

            // Rotate the pointer to face the direction
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pointerUI.rotation = Quaternion.Euler(0, 0, angle - 90); // minus 90 if the arrow points up
        }
        else
        {
            pointerUI.gameObject.SetActive(false);
        }
    }
}