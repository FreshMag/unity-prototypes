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
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSwitch = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwitch)
        {
            RotatePlatform(-90);
            transform.parent.gameObject.SetActive(false);
            newAxis.gameObject.SetActive(true);
            canSwitch = false;
        }
    }

    void RotatePlatform(float angle)
    {
        FindAnyObjectByType<MousePlatform>().transform.Rotate(0, 0, angle);
    }
}
