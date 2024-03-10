using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    public AudioClip swordSFX;
    private bool isDead = false;
    public PlayerController playa;
    // UI Animator for health
    [SerializeField] private Animator healthAnimator;
    // Speed of enemy
    public float moveSpeed;
    private float tempMoveSpeed;

    [SerializeField] EnemyWaveManager enemyWaveManager;

    // Create an animator controller
    private Animator animator;

    // Reference to the player's position
    [SerializeField]
    private Transform playerPosition;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Check if the enemy is moving
    private bool isMoving;

    private float summonTime = 20f;

    // Radius for collision detection
    [SerializeField]
    private float collisionRadius = 0.5f;

    // Bool to check if we are attacking
    private bool isSwording = false;

    // Bool to check if the enemy is currently summoning
    private bool isSummoning = false;

    // Public fields for game objects
    public GameObject summonObject1;
    public GameObject summonObject2;
    public GameObject summonObject3;
    public GameObject summonObject4;
    public GameObject item;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private int health = 10;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Initialize the player position if not set
        if (playerPosition == null)
        {
            Debug.LogError("Player position is not set in the inspector!");
        }

        // Start summoning every 8 seconds
        if(!IsOriginal())
        {
            InvokeRepeating("Summon", 0f, summonTime);
        }
        

        tempMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            moveSpeed = 0;
        }
        if (!isSwording && !isSummoning && !playa.isInside && !IsOriginal()  && !isDead)
        {
            // Move towards the player's position
            MoveTowardsPlayer();
        }

        if(playa.isInside)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = 6f;
        }
        if(IsOriginal())
        {
            moveSpeed = 0;
        }

        
    }

    private bool IsOriginal()
    {
        // Check if the name of the GameObject is exactly "Grunt"
        return gameObject.name == "General";
    }


    // Function to move towards the player's position
    private void MoveTowardsPlayer()
    {
        if (!isMoving)
        {
            // Calculate direction towards the player
            Vector3 direction = (playerPosition.position - transform.position).normalized;

            // Use the calculated direction for animations
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            // Calculate the target position
            Vector3 targetPos = transform.position + direction * moveSpeed * Time.deltaTime;

            if (IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }
    }

    // Coroutine for movement
    IEnumerator Move(Vector3 targetPos)
    {
        // Set moving to be true
        isMoving = true;

        // While the enemy is not at the target position
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Move towards the target position at the given speed
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // We are no longer moving
        isMoving = false;
    }

    // Check if the position is walkable
    private bool IsWalkable(Vector3 targetPos)
    {
        // If collision detected, no walk allowed; if not detected, you may continue.
        return Physics2D.OverlapCircle(targetPos, collisionRadius, LayerMask.GetMask("SolidObjects")) == null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (health == 0)
            {
                // Trigger death animation
                animator.SetTrigger("DIEBITCH");

                // Change speed to 0
                moveSpeed = 0f;

                // Start a coroutine to wait for the animation to complete
                StartCoroutine(WaitForAnimation());
            }
            else
            {
                health--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSwording)
        {
            isSwording = true;
            animator.SetTrigger("Sword");
            if(!IsOriginal())
            {
                audioSource.PlayOneShot(swordSFX, 0.2f);
            }
            StartCoroutine(WaitForSword());
        }
    }

    // Function to handle summoning
    private void Summon()
    {
        if (!isSummoning && !playa.isInside)
        {
            // Set isSummoning to true to stop movement during summoning
            isSummoning = true;

            // Stop moving
            isMoving = false;

            // Trigger Summon animation
            animator.SetTrigger("Summon");

            // Wait for the length of the summon animation
            StartCoroutine(WaitForSummonAnimation());

            // Spawn instances of the summon objects
            Instantiate(summonObject1, transform.position, Quaternion.identity);
            Instantiate(summonObject2, transform.position, Quaternion.identity);
            Instantiate(summonObject3, transform.position, Quaternion.identity);
            Instantiate(summonObject4, transform.position, Quaternion.identity);

        }
    }

    IEnumerator WaitForSword()
    {
        yield return new WaitForSeconds(1f);
        isSwording = false;
    }

    IEnumerator WaitForAnimation()
    {
        isDead = true;
        moveSpeed = 0f;
        // Wait for the length of the death animation
        yield return new WaitForSeconds(3f);

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

        // Notify the enemy wave manager about the death
        enemyWaveManager.counter--;

        //spawn item
        Instantiate(item, transform.position, Quaternion.identity);

        // Destroy the enemy gameObject after the animation is complete
        Destroy(gameObject);

        
    }

    IEnumerator WaitForSummonAnimation()
    {
        // Wait for the length of the summon animation
        yield return new WaitForSeconds(1f);

        // Set isSummoning to false to resume movement
        isSummoning = false;
    }
}
