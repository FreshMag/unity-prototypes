using UnityEngine;

// Component that manages the health of an entity. It changes the slider value to indicate health status.
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Maximum health value
    public int currentHealth; // Current health value
    public UnityEngine.UI.Slider healthSlider; // Reference to the UI slider

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to maximum
        UpdateHealthSlider(); // Update the slider to reflect initial health
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            // Assuming the bullet has a script that handles damage
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage); // Call TakeDamage method with bullet's damage value
                Destroy(collision.gameObject); // Destroy the bullet after it hits
            }
        }
    }

    // Method to apply damage to the entity
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount
        if (currentHealth < 0) currentHealth = 0; // Ensure health doesn't go below zero
        UpdateHealthSlider(); // Update the slider after taking damage

        // Flash red screen effect
        StartCoroutine(FlashRedScreen());

        if (currentHealth <= 0)
        {
            Die(); // Call die method if health reaches zero
        }
    }

    private System.Collections.IEnumerator FlashRedScreen()
    {
        // Create a red overlay panel if it doesn't exist
        GameObject redOverlay = GameObject.Find("RedDamageOverlay");
        if (redOverlay == null)
        {
            redOverlay = new GameObject("RedDamageOverlay");
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                redOverlay.transform.SetParent(canvas.transform, false);
                UnityEngine.UI.Image image = redOverlay.AddComponent<UnityEngine.UI.Image>();
                image.color = new Color(0.5f, 0f, 0f, 0.3f); // Darker red with less transparency
                RectTransform rect = redOverlay.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
        }

        UnityEngine.UI.Image overlayImage = redOverlay.GetComponent<UnityEngine.UI.Image>();
        
        // Gradually fade in the overlay
        redOverlay.SetActive(true);
        for (float t = 0; t < 0.2f; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 0.3f, t / 0.2f);
            overlayImage.color = new Color(0.5f, 0f, 0f, alpha);
            yield return null;
        }
        
        // Hold the effect briefly
        yield return new WaitForSeconds(0.15f);
        
        // Gradually fade out the overlay
        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.3f, 0f, t / 0.3f);
            overlayImage.color = new Color(0.5f, 0f, 0f, alpha);
            yield return null;
        }
        
        // Hide the overlay
        redOverlay.SetActive(false);
    }

    // Method to update the health slider UI
    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth; // Set slider value based on current health
        }
    }

    // Method called when the entity dies
    private void Die()
    {
        Debug.Log("Entity has died."); // Log death message
        Destroy(gameObject); // Destroy the game object
    }

}
