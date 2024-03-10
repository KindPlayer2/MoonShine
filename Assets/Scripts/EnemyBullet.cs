using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;

    private Vector2 bulletDirection;

    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized;
    }

    void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);

        // Destroy the bullet when it goes out of the screen
        if (!GetComponent<Renderer>().isVisible && !IsOriginalBullet())
        {
            Destroy(gameObject);
        }
    }

    // Function to check if the bullet is the original or a clone
    private bool IsOriginalBullet()
    {
        // Check if the name of the GameObject is exactly "GruntBullet"
        return gameObject.name == "GruntBullet";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle trigger collisions with other objects
        if (other.CompareTag("Player"))
        {
            // Do something when the bullet enters a trigger collider (e.g., an enemy)
            Destroy(gameObject);
        }
    }
}
