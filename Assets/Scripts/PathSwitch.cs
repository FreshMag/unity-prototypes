using UnityEngine;

public class PathSwitch : MonoBehaviour
{
    public GameObject newAxis;
    private bool canSwitch = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canSwitch = true;
            other.GetComponent<MousePlatform>()?.switchIndicator.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canSwitch = false;
            other.GetComponent<MousePlatform>()?.switchIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwitch)
        {
            transform.parent.gameObject.SetActive(false);
            newAxis.gameObject.SetActive(true);
            //RotatePlatformToMatchNewAxis();
            canSwitch = false;
        }
    }

    void RotatePlatformToMatchNewAxis()
    {
        MousePlatform platform = FindAnyObjectByType<MousePlatform>();
        if (platform == null || newAxis == null) return;

        // Get the up directions
        Vector3 targetUp = newAxis.transform.GetComponentInChildren<Path>().transform.up;

        Vector3 currentUp = platform.transform.up;

        // Calculate rotation from current up to target up
        Quaternion rotation = Quaternion.FromToRotation(currentUp, targetUp);
        // Apply rotation to the platform
        platform.transform.rotation = rotation * platform.transform.rotation;
    }
}
