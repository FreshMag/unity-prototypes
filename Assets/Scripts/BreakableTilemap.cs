using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class BreakableTilemap : MonoBehaviour
{
    public Tilemap tilemap;

    [Header("Power ups")]
    [Range(0f, 1f)]
    public float powerUpChance;

    public List<GameObject> powerUpPrefabs;


    private float invisibleDuration = 3f; // Maximum time out of camera that triggers deletion
    private float invisibleTimer = 0f;
    private bool isInvisible = false;

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }
    }

    void Update()
    {
        if (isInvisible)
        {
            invisibleTimer += Time.deltaTime;
            if (invisibleTimer >= invisibleDuration)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider is the ball
        if (collision.gameObject.CompareTag("Ball"))
        {

            // Adjust the hit point slightly into the tile
            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point + (Vector2)contact.normal * 0.01f;

            Vector3Int tilePos = tilemap.WorldToCell(hitPoint);

            if (tilemap.HasTile(tilePos))
            {
                tilemap.SetTile(tilePos, null);
                if (Random.Range(0f, 1f) < powerUpChance)
                {
                    if (powerUpPrefabs.Count > 0)
                    {
                        var powerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Count)];
                        Instantiate(powerUp, tilePos, Quaternion.identity);
                    }
                }
            }
        }
    }

    void OnBecameInvisible()
    {
        isInvisible = true;
        invisibleTimer = 0f; // Inizia il conteggio da zero
    }

    void OnBecameVisible()
    {
        isInvisible = false;
    }
}
