using System.Collections.Generic;
using UnityEngine;

public class SingleDetector : MonoBehaviour
{
    public bool hasGround => groundColliders.Count > 0;
    private HashSet<Collider2D> groundColliders = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            groundColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            groundColliders.Remove(collision);
    }
}
