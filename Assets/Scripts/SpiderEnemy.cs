using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    private int currentWaypointIndex = 0;
    private bool forward = true;

    [Header("Detection")]
    public float detectionRadius = 10f;
    public Transform player;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float shootInterval = 1.5f;
    public float pauseBeforeShooting = 0.2f; // Pausa prima di sparare
    public int numberOfBullets = 2; // Numero di proiettili da sparare
    public float totalSpreadAngle = 30f; // Angolo totale di dispersione dei proiettili
    private float shootTimer = 0f;
    private bool isPausing = false;
    private float pauseTimer = 0f;

    private int health = 2; // Example health value, can be adjusted

    void Update()
    {
        if (!isPausing)
        {
            MoveAlongPath();
        }
        DetectAndShoot();
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            if (forward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = waypoints.Length - 2;
                    forward = false;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 1;
                    forward = true;
                }
            }
        }
    }

    void DetectAndShoot()
    {
        if (isPausing)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseBeforeShooting)
            {
                Shoot();
                isPausing = false;
                pauseTimer = 0f;
                shootTimer = 0f;
            }
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                // Inizia la pausa prima di sparare
                isPausing = true;
                pauseTimer = 0f;
            }
        }
        else
        {
            shootTimer = 0f; // Reset timer if player is out of range
            isPausing = false;
            pauseTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            // Make the color of the sprite darker
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 0.5f; // Set alpha to 50% for a darker effect
                spriteRenderer.color = color;
            }
            // Decrease health
            health--;
            if (health <= 0)
            {
                Destroy(gameObject); // Destroy the spider enemy when health reaches 0
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab && shootPoint && numberOfBullets > 0)
        {
            Vector2 direction = (player.position - shootPoint.position).normalized;
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Se c'è solo un proiettile, spara direttamente verso il giocatore
            if (numberOfBullets == 1)
            {
                var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                bullet.transform.up = direction;
                bullet.GetComponent<Bullet>().direction = Vector2.up;
            }
            else
            {
                // Calcola l'angolo tra ogni proiettile
                float angleStep = totalSpreadAngle / (numberOfBullets - 1);
                float startAngle = baseAngle - totalSpreadAngle / 2;
                
                for (int i = 0; i < numberOfBullets; i++)
                {
                    float currentAngle = startAngle + (angleStep * i);
                    Vector2 bulletDirection = new Vector2(
                        Mathf.Cos(currentAngle * Mathf.Deg2Rad), 
                        Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                    );
                    
                    var bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                    bullet.transform.up = bulletDirection;
                    bullet.GetComponent<Bullet>().direction = Vector2.up;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
