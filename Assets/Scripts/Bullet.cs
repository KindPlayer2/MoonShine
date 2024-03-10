using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Speed of the bullet
    public float bulletSpeed = 5f;

    // Direction of the bullet
    private Vector2 bulletDirection;

    // Set the direction of the bullet
    public void SetDirection(Vector2 direction)
    {
        bulletDirection = direction.normalized;
    }

    void Update()
    {
        // Move the bullet in the specified direction
        transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);

        // Destroy the bullet when it goes out of the screen (you may want to adjust this based on your game design)
        if (!GetComponent<Renderer>().isVisible && !IsOriginalBullet())
        {
            Destroy(gameObject);
        }
    }

        // Function to check if the bullet is the original or a clone
    private bool IsOriginalBullet()
    {
        // Check if the name of the GameObject is exactly "Bullet"
        return gameObject.name == "Bullet";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collisions with other objects
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Do something when the bullet collides with an enemy
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle trigger collisions with other objects
        if (other.CompareTag("Enemy"))
        {
            // Do something when the bullet enters a trigger collider (e.g., an enemy)
            Destroy(gameObject);
        }
    }
    
}
