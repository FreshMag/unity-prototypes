using UnityEngine;


public class WaterBehavior : MonoBehaviour
{
    public float speed = 5f;
    public float spreadForce = 3f;
    public float forceDown = 5f;

    private Rigidbody2D rb;
    private bool isSpreading = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isSpreading) return; // prevent multiple triggers

        if (other.CompareTag("Player"))
        {
            SpreadOut();
        }
        else if (other.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = other.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                ballRb.AddForce(Vector2.down * forceDown, ForceMode2D.Impulse);
            }
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            SpreadOut();
        }
    }

    void SpreadOut()
    {
        isSpreading = true;
        Vector2 spreadDirection = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        rb.linearVelocity = spreadDirection * spreadForce + Vector2.down * 0.5f;
        Destroy(gameObject, 1f);
    }
}
