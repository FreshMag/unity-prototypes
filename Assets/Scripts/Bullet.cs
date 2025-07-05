using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;
    public int damage = 16; // Damage value for the bullet
    public Vector2 direction = Vector2.down;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {

        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Fai danni al player
            Destroy(gameObject);
        }
    }
}
