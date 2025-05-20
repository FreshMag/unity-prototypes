using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(PowerUp))]
public class PowerUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PowerUp powerUp = (PowerUp)target;

        powerUp.type = (PowerUpType)EditorGUILayout.EnumPopup("Power Up Type", powerUp.type);

        switch (powerUp.type)
        {
            case PowerUpType.SpeedUpBall:
                powerUp.speedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", powerUp.speedMultiplier);
                break;

            case PowerUpType.LargerBall:
                powerUp.ballSizeMultiplier = EditorGUILayout.FloatField("Ball Size Multiplier", powerUp.ballSizeMultiplier);
                break;

            case PowerUpType.LargerPlatform:
                powerUp.platformSizeMultiplier = EditorGUILayout.FloatField("Platform Size Multiplier", powerUp.platformSizeMultiplier);
                break;

            case PowerUpType.SpawnBall:
                powerUp.ballPrefab = (GameObject)EditorGUILayout.ObjectField("Ball Prefab", powerUp.ballPrefab, typeof(GameObject), false);
                powerUp.numberOfBallsToSpawn = EditorGUILayout.IntField("Balls to Spawn", powerUp.numberOfBallsToSpawn);
                break;
        }

        EditorUtility.SetDirty(powerUp);
    }
}
#endif
public enum PowerUpType
{
    SpeedUpBall,
    LargerBall,
    LargerPlatform,
    SpawnBall
}

public class PowerUp : MonoBehaviour
{
    public float maxForce = 10.5f;
    public float maxAngle = 50f;

    public PowerUpType type;

    [Header("Speed Up Settings")]
    public float speedMultiplier = 1.5f;

    [Header("Larger Ball Settings")]
    public float ballSizeMultiplier = 1.5f;

    [Header("Larger Platform Settings")]
    public float platformSizeMultiplier = 1.5f;

    [Header("Spawn Ball Settings")]
    public GameObject ballPrefab;
    public int numberOfBallsToSpawn = 1;

    void Start()
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        Vector2 force = direction * maxForce;

        GetComponent<Rigidbody2D>().linearVelocity = force;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    GameObject GetBall()
    {
        return GameObject.FindGameObjectWithTag("Ball");
    }

    void ApplyPowerUp(GameObject player)
    {
        switch (type)
        {
            case PowerUpType.SpeedUpBall:
                GetBall().GetComponent<BallComponent>().initialSpeed *= 1.5f;
                break;
            case PowerUpType.LargerBall:
                GetBall().transform.localScale *= ballSizeMultiplier;
                break;
            case PowerUpType.LargerPlatform:
                player.transform.localScale *= platformSizeMultiplier;
                break;
            case PowerUpType.SpawnBall:
                var ballTransform = GetBall().transform;
                var newBall = Instantiate(ballPrefab, ballTransform.position, ballTransform.rotation);
                newBall.GetComponent<BallComponent>().EnableMovement();
                break;
        }
    }
}
