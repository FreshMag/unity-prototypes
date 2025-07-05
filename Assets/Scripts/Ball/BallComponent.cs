using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallComponent : MonoBehaviour
{
    public float initialSpeed = 5f;
    
    private Rigidbody2D rb;
    
    [HideInInspector]
    public bool isLaunched = false;
    private bool hasStarted = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // Wait for launch
        hasStarted = true;
        if (isLaunched)
        {
            LaunchBall();
        }
    }

    void Update()
    {
        // Launch the ball with Space key
        if (!isLaunched && Input.GetMouseButtonDown(0))
        {
            LaunchBall();
        }
    }

    // this ugly workaround is necessary to launch a ball that hasn't been "Started" yet.
    public void EnableMovement()
    {
        if (hasStarted)
        {
            LaunchBall();
        }
        else
        {
            isLaunched = true;
        }
    }

    void LaunchBall()
    {
        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isLaunched = true;

        // Launch upward and slightly to the right
        rb.linearVelocity = new Vector2(1f, 1f).normalized * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Slight tweak to avoid boring straight horizontal/vertical bounces
        Vector2 vel = rb.linearVelocity;
        if (Mathf.Abs(vel.y) < 0.1f)
        {
            vel.y += 0.2f * Mathf.Sign(vel.y == 0 ? 1 : vel.y);
        }
        if (Mathf.Abs(vel.x) < 0.1f)
        {
            vel.x += 0.2f * Mathf.Sign(vel.x == 0 ? 1 : vel.x);
        }

        rb.linearVelocity = vel.normalized * initialSpeed;
    }
}