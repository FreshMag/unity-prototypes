using UnityEngine;
using System.Collections;

public class CrowEnemy : MonoBehaviour
{
    [Header("Flying Settings")]
    public float flySpeed = 3f;
    public float hoverHeight = 2f;
    public float hoverAmplitude = 0.5f;
    public float hoverFrequency = 2f;
    
    [Header("Detection")]
    public float detectionRadius = 8f;
    public Transform player;
    
    [Header("Attack")]
    public float attackPreparationTime = 1f;
    public float diveSpeed = 12f;
    public float bounceForce = 8f;
    public int crowDamage = 30; // Damage dealt to the player when hit
    
    [Header("Perching")]
    public float perchSearchRadius = 5f;
    public LayerMask blockLayerMask = -1;
    public float returnSpeed = 2f; // Velocità per tornare alla posizione iniziale
    
    private enum CrowState
    {
        Resting,        // Stato iniziale, fermo nella posizione di partenza
        PreparingAttack,
        Diving,
        Bouncing,
        Returning       // Torna alla posizione iniziale
    }
    
    private CrowState currentState = CrowState.Resting;
    private Rigidbody2D rb;
    private Vector2 startingPosition; // Posizione di partenza
    private float hoverTimer;
    private bool isPlayerInRange;
    private Vector2 diveDirection;
    private int health = 2;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        // Salva la posizione di partenza
        startingPosition = transform.position;
        
        // Inizia nello stato di riposo
        currentState = CrowState.Resting;
        rb.linearVelocity = Vector2.zero;
    }
    
    void Update()
    {
        CheckPlayerDistance();
        
        switch (currentState)
        {
            case CrowState.Resting:
                HandleResting();
                break;
            case CrowState.PreparingAttack:
                HandlePreparingAttack();
                break;
            case CrowState.Diving:
                HandleDiving();
                break;
            case CrowState.Bouncing:
                HandleBouncing();
                break;
            case CrowState.Returning:
                HandleReturning();
                break;
        }
    }
    
    void CheckPlayerDistance()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool wasPlayerInRange = isPlayerInRange;
        isPlayerInRange = distanceToPlayer <= detectionRadius;
        
        // Se il player entra nel raggio e il crow è a riposo
        if (isPlayerInRange && !wasPlayerInRange && currentState == CrowState.Resting)
        {
            StartCoroutine(PrepareAttack());
        }
        // Se il player esce dal raggio e il crow non sta attaccando
        else if (!isPlayerInRange && wasPlayerInRange && currentState != CrowState.Diving && currentState != CrowState.PreparingAttack)
        {
            currentState = CrowState.Returning;
        }
    }
    
    void HandleResting()
    {
        // Rimane fermo nella posizione di partenza con un leggero ondeggiamento
        hoverTimer += Time.deltaTime;
        float hoverOffset = Mathf.Sin(hoverTimer * hoverFrequency) * hoverAmplitude * 0.2f;
        Vector2 targetPosition = startingPosition + Vector2.up * hoverOffset;
        
        // Movimento molto lento verso la posizione target per l'ondeggiamento
        Vector2 direction = (targetPosition - (Vector2)transform.position);
        if (direction.magnitude > 0.05f)
        {
            rb.linearVelocity = direction.normalized * returnSpeed * 0.3f;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    void HandleReturning()
    {
        // Torna alla posizione di partenza
        Vector2 direction = (startingPosition - (Vector2)transform.position);
        
        if (direction.magnitude < 0.5f)
        {
            // È arrivato alla posizione di partenza
            currentState = CrowState.Resting;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            // Si muove verso la posizione di partenza
            rb.linearVelocity = direction.normalized * returnSpeed;
        }
    }
    
    IEnumerator PrepareAttack()
    {
        currentState = CrowState.PreparingAttack;
        rb.linearVelocity = Vector2.zero;
        
        float elapsed = 0f;
        Vector2 originalPosition = transform.position;
        
        // Ondeggia su e giù mentre si prepara
        while (elapsed < attackPreparationTime)
        {
            elapsed += Time.deltaTime;
            float hoverOffset = Mathf.Sin(elapsed * hoverFrequency * 2f) * hoverAmplitude * 0.5f;
            Vector2 targetPos = originalPosition + Vector2.up * hoverOffset;
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
            yield return null;
        }
        
        // Inizia la picchiata
        if (player != null)
        {
            diveDirection = (player.position - transform.position).normalized;
            currentState = CrowState.Diving;
        }
        else
        {
            currentState = CrowState.Returning;
        }
    }
    
    void HandlePreparingAttack()
    {
        // Gestito dalla coroutine
    }
    
    void HandleDiving()
    {
        rb.linearVelocity = diveDirection * diveSpeed;
    }
    
    void HandleBouncing()
    {
        // Lo stato di rimbalzo viene gestito automaticamente dopo la collisione
        // Torna a controllare se il player è ancora nel raggio, altrimenti torna alla base
        if (rb.linearVelocity.magnitude < 2f)
        {
            if (isPlayerInRange)
            {
                // Se il player è ancora nel raggio, prepara un nuovo attacco
                StartCoroutine(PrepareAttack());
            }
            else
            {
                // Se il player non è più nel raggio, torna alla base
                currentState = CrowState.Returning;
            }
        }
    }
    
    Collider2D FindNearestBlock()
    {
        // Questa funzione non è più necessaria nel nuovo approccio
        // ma la mantengo per compatibilità futura
        GameObject[] blockObjects = GameObject.FindGameObjectsWithTag("Blocks");
        Collider2D nearest = null;
        float nearestDistance = float.MaxValue;
        
        foreach (GameObject blockObj in blockObjects)
        {
            float distance = Vector2.Distance(transform.position, blockObj.transform.position);
            if (distance <= perchSearchRadius && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = blockObj.GetComponent<Collider2D>();
            }
        }
        
        return nearest;
    }
    
    bool CanMoveInDirection(Vector2 direction)
    {
        // Effettua un raycast per controllare se ci sono ostacoli
        float rayDistance = 1.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance);
        
        if (hit.collider != null)
        {
            // Se colpisce un muro o terreno, non può muoversi
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ground"))
            {
                return false;
            }
        }
        
        return true;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Rimbalza quando colpisce muri, terreno o player
        if (collision.CompareTag("Wall") || collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            if (currentState == CrowState.Diving)
            {
                // Calcola direzione di rimbalzo
                Vector2 bounceDirection = Vector2.Reflect(diveDirection, GetCollisionNormal(collision));
                rb.linearVelocity = bounceDirection * bounceForce;
                currentState = CrowState.Bouncing;
                
                // Danneggia il player se lo colpisce
                if (collision.CompareTag("Player"))
                {
                    Health playerHealth = collision.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(crowDamage);
                    }
                }
            }
            else if (currentState == CrowState.Returning)
            {
                // Se colpisce un muro mentre torna, continua a provare
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        // Prende danno dai proiettili
        if (collision.CompareTag("Damage"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(collision.gameObject);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Gestisce le collisioni fisiche (non trigger) con muri e terreno
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            if (currentState == CrowState.Returning)
            {
                // Se colpisce un muro mentre torna, si sposta leggermente
                rb.linearVelocity = Vector2.zero;
                Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
                transform.position = (Vector2)transform.position + pushDirection * 0.5f;
            }
        }
    }
    
    Vector2 GetCollisionNormal(Collider2D collision)
    {
        // Calcola la normale della collisione in modo semplificato
        Vector2 directionToCollision = (collision.transform.position - transform.position).normalized;
        return -directionToCollision;
    }
    
    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Aggiunge un piccolo effetto prima di distruggere
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualizza il raggio di detection
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Visualizza il raggio di ricerca blocchi
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, perchSearchRadius);
    }
}
