using System.Collections;
using UnityEngine;

public class Family : MonoBehaviour
{
    // Variables for wandering
    public float wanderTime = 3f;
    public float pauseTime = 1.017f;
    public float moveSpeed = 2f;

    private bool isMoving = false;

    // Variables for animation and landmine deployment
    private Animator animator;

    // Variables for triggering die animation
    private bool isDead = false;

    private Vector3 constantDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the die animation and destruction after 2 seconds
        if (!isDead && !isMoving)
        {
            StartCoroutine(Wander());
        }

        UpdateAnimator(constantDirection);
    }

    IEnumerator Wander()
    {
        isMoving = true;
        
        animator.SetTrigger("moveAgain");
        // Store the facing direction before moving
        Vector3 facingDirection = GetRandomDirection();

        // Move in a straight line for a few seconds
        yield return StartCoroutine(MoveInDirection(facingDirection, wanderTime));

        // Trigger the animation deploy
        animator.SetTrigger("Idle");

        // Wait for 2 seconds
        yield return new WaitForSeconds(1.5f);

        

        isMoving = false;

        
    }

    Vector3 GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 8);
        Vector3[] directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            new Vector3(1, 1).normalized,
            new Vector3(1, -1).normalized,
            new Vector3(-1, 1).normalized,
            new Vector3(-1, -1).normalized
        };
        return directions[randomIndex];
    }

    IEnumerator MoveInDirection(Vector3 direction, float duration)
    {
        constantDirection = direction;

        float elapsedTime = 0f;
        while (elapsedTime < duration && !isDead)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            UpdateAnimator(direction);
            yield return null;
        }
    }

    void UpdateAnimator(Vector3 direction)
    {
        animator.SetFloat("moveX", direction.x);
        //animator.SetFloat("moveY", direction.y);
    }

    IEnumerator Die()
    {
        // Trigger the "Die" animation
        animator.SetTrigger("Die");

        // Wait for 2 seconds
        yield return new WaitForSeconds(1.5f);

        // Destroy the game object
        Destroy(gameObject);
        
    }

    // Function to handle the bullet trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            isDead = true;

            // Trigger the "Die" animation using SetTrigger("Die")
            animator.SetTrigger("Die");

            // Wait for 2 seconds
            StartCoroutine(Die());

        }

    }
}
