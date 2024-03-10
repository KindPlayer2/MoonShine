using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public AudioClip laserSFX;
    public AudioClip deathSFX;
    public AudioClip swordSFX;

    private bool isDead = false;

    public GameObject item;

    // UI Animator for health
    [SerializeField] private Animator healthAnimator;

    public PlayerController playa;

    // Speed of enemy
    public float moveSpeed;

    private float tempMoveSpeed;


    // Create an animator controller
    private Animator animator;

    // Reference to the player's position
    [SerializeField]
    private Transform playerPosition;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Check if the enemy is moving
    private bool isMoving;

    // Radius for collision detection
    [SerializeField]
    private float collisionRadius = 0.5f;

    //If laser firing animation is playing
    private bool laserFiring;

    // Timer variables
    private float timer = 8f;
    private float laserAnimationDuration = 1.78f;

    // Reference to the laser object
    public GameObject laserPrefab;

    // Laser spawn offset from the enemy
    public Vector3 laserSpawnOffset;

    // Laser duration
    public float laserDuration = 1.56f;

    // Laser spawn delay
    public float laserSpawnDelay = 0.22f;

    private int health = 10;


    //Bool to check if we are attacking
    private bool isSwording = false;

    [SerializeField] EnemyWaveManager enemyWaveManager;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Initialize the player position if not set
        if (playerPosition == null)
        {
            Debug.LogError("Player position is not set in the inspector!");
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
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if(!laserFiring && !isSwording && !playa.isInside  && !isDead)
        {

            // Move towards the player's position
            MoveTowardsPlayer();

            // Update timer
            timer -= Time.deltaTime;
            
            if(timer <= 5f && !IsOriginal())
            {
                // Play the shoot sound effect
                audioSource.PlayOneShot(laserSFX, 0.1f);
            }
            if (timer <= 0f)
            {
                // Start laser firing animation
                StartLaserFiringAnimation();

                // Reset timer
                timer = 10f;
                
            }//end if

            
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
        return gameObject.name == "Robot";
    }


    private void StartLaserFiringAnimation()
    {
        if (!laserFiring)
        {
            StartCoroutine(ActivateLaserFiring());
        }
    }

    IEnumerator ActivateLaserFiring()
    {
        laserFiring = true;

        // Set the "LaserFiring" boolean to true in the animator
        animator.SetTrigger("LaserFire");

        // Wait for the specified delay before spawning the laser
        yield return new WaitForSeconds(laserSpawnDelay);

        // Spawn the laser
        SpawnLaser();

        // Wait for the length of the laser firing animation
        yield return new WaitForSeconds(laserAnimationDuration);

        laserFiring = false;
    }

    void SpawnLaser()
    {
        // Calculate direction towards the player
        Vector3 direction = (playerPosition.position - transform.position).normalized;

        // Calculate the position to spawn the laser
        Vector3 spawnPosition = transform.position + direction * laserSpawnOffset.x + new Vector3(0, 0, laserSpawnOffset.y);

        // Instantiate the laser object
        GameObject laserInstance = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);

        // Rotate the laser to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the laser after the specified duration
        Destroy(laserInstance, laserDuration);
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
        // If collision detected, no walk allowed; if not detected, you may continue. Happy days!
        if (Physics2D.OverlapCircle(targetPos, collisionRadius, LayerMask.GetMask("SolidObjects")) != null)
        {
            // If we are touching the wall, return false
            return false;
        }
        else
        {
            // Otherwise, return true
            return true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(health == 0)
            {
                // Trigger death animation
                animator.SetTrigger("DIEBITCH");

                if(!IsOriginal())
                {
                    // Play the shoot sound effect
                    audioSource.PlayOneShot(deathSFX, 0.1f);
                }
                
                //change speed to 0
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
}