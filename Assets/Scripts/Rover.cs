using System.Collections;
using UnityEngine;

public class Rover : MonoBehaviour
{
    public AudioClip explodeSFX;
    private bool isDead = false;
    public GameObject item;
    // UI Animator for health
    [SerializeField] private Animator healthAnimator;
    public PlayerController playa;
    // Variables for wandering
    public float wanderTime = 3f;
    public float pauseTime = 1.017f;
    public float moveSpeed = 2f;
    private float tempMoveSpeed;

    [SerializeField] EnemyWaveManager enemyWaveManager;

    public int health;

    private bool isMoving = false;

    // Variables for animation and landmine deployment
    private Animator animator;
    public GameObject landminePrefab;

    // Variables for triggering die animation

    private Vector3 constantDirection;

    private AudioSource audioSource;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        tempMoveSpeed = moveSpeed;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            moveSpeed = 0;
        }
        // Check for the die animation and destruction after 2 seconds
        if (!isDead && !isMoving && !playa.isInside && !IsOriginal()  && !isDead)
        {
            StartCoroutine(Wander());
        }

        UpdateAnimator(constantDirection);

        if(playa.isInside)
        {
            moveSpeed = 0;
        }
        else
        {
            if(!IsOriginal())
            {
                moveSpeed = 3f;
            }
            
        }
        if(IsOriginal())
        {
            moveSpeed = 0;
        }

        
    }

    private bool IsOriginal()
    {
        // Check if the name of the GameObject is exactly "Rover or Tank"
        if(gameObject.name == "Tank")
        {
            return gameObject.name == "Tank";
        }
        if(gameObject.name == "Rover")
        {
            return gameObject.name == "Rover";
        }
        else
        {
            return false;
        }
        
    }

    IEnumerator Wander()
    {
        isMoving = true;

        // Store the facing direction before moving
        Vector3 facingDirection = GetRandomDirection();

        // Move in a straight line for a few seconds
        yield return StartCoroutine(MoveInDirection(facingDirection, wanderTime));

        if(!playa.isInside)
        {
            // Trigger the animation deploy
            animator.SetTrigger("Deploy");

            // Wait for 2 seconds
            yield return new WaitForSeconds(1.5f);

            animator.SetTrigger("moveAgain");

            // Spawn an instance of a Landmine in the current position
            Instantiate(landminePrefab, transform.position, Quaternion.identity);
        }

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
        animator.SetFloat("moveY", direction.y);
    }

    IEnumerator Die()
    {
        isDead = true;
        moveSpeed = 0f;
        // Trigger the "Die" animation
        animator.SetTrigger("Die");

        if(gameObject.name != "Rover" && !playa.isInside)
        {
            // Play the shoot sound effect
            //audioSource.PlayOneShot(explodeSFX, 0.1f);
        }

        if(playa.healthBoost)
        {
            if(playa.health > 4)
            {
                healthAnimator.SetTrigger("Fish");
                playa.health = 8;
            }
            else
            {
                healthAnimator.SetTrigger("HOK");
                playa.health = 8;
            }
            
        }

        // Wait for 2 seconds
        yield return new WaitForSeconds(1.5f);

        // Notify the enemy wave manager about the death
        enemyWaveManager.counter--;

        //spawn item
        Instantiate(item, transform.position, Quaternion.identity);

        // Destroy the game object
        Destroy(gameObject);
        
    }

    // Function to handle the bullet trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(health == 0)
            {
                isDead = true;

                // Trigger the "Die" animation using SetTrigger("Die")
                animator.SetTrigger("Die");

                // Wait for 2 seconds
                StartCoroutine(Die());
            }
            else
            {
                health--;
            }
        }

    }
}
