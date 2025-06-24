using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BallComponent), typeof(Rigidbody2D))]
public class BallManualMovement : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 2f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 15f;      // per second
    public float baseStaminaDrain = 10f;      // per second
    public float holdMultiplier = 1.5f;       // drain multiplier when holding input
    public Slider staminaSlider;

    private Rigidbody2D rb;
    private BallComponent ballComponent;

    private float currentStamina;
    private float heldInputDuration = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballComponent = GetComponent<BallComponent>();
        currentStamina = maxStamina;
        UpdateStaminaUI();
    }

    void Update()
    {
        if (!ballComponent.isLaunched)
            return;

        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input += Vector2.up;
        if (Input.GetKey(KeyCode.S)) input += Vector2.down;
        if (Input.GetKey(KeyCode.A)) input += Vector2.left;
        if (Input.GetKey(KeyCode.D)) input += Vector2.right;

        bool isPushing = input != Vector2.zero;

        if (isPushing && currentStamina > 0f)
        {
            heldInputDuration += Time.deltaTime;

            // Calculate the current frame's stamina drain
            float drain = baseStaminaDrain * Mathf.Lerp(1f, holdMultiplier, heldInputDuration / 3f);

            // ⛔ Prevent pushing BEFORE drain if stamina won't be enough
            if (currentStamina >= drain * Time.deltaTime)
            {
                rb.AddForce(input.normalized * pushForce, ForceMode2D.Impulse);
                currentStamina -= drain * Time.deltaTime;
            }
            else
            {
                // Stamina too low this frame — don't move, stop input
                heldInputDuration = 0f;
            }

            currentStamina = Mathf.Max(0f, currentStamina);
        }
        else
        {
            // Not holding movement — reset input duration
            heldInputDuration = 0f;

            // Regen stamina
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }

        UpdateStaminaUI();
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }
}
