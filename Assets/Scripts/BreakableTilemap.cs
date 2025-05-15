using UnityEngine;

using UnityEngine.Tilemaps;

public class BreakableTilemap : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
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
            }
        }
    }
}
