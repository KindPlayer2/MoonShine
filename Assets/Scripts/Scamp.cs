using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scamp : MonoBehaviour
{
    public GameObject item;
    public AudioClip swordSFX;
    // UI Animator for health
    [SerializeField] private Animator healthAnimator;
    public PlayerController playa;
    public Transform playerPosition;
    public float moveSpeed = 5f;
    private float tempMoveSpeed;
    public float stopDistance = 10f;
    public float waitTime = 1f;
    public GameObject bulletPrefab; // Serialized field for the enemy bullet prefab
    public float shootCooldown = 3f; // Cooldown time for shooting
    private Animator animator;
    private bool isMoving = false;
    private Vector3 shootingDirection;

    private bool isDead = false;

    [SerializeField] EnemyWaveManager enemyWaveManager;

    
    //See if we're attacking
    private bool isSwording = false;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


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

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        //Debug.Log(distanceToPlayer);

        
        //Check if we are in position before we shoot
        if (!isSwording && !playa.isInside && !IsOriginal() && !isDead)
        {

            //go to player NOW
            MoveTowardsPlayer();

        }//end if 

        if(isDead)
        {
            moveSpeed = 0;
        }
        
        if(playa.isInside)
        {
            moveSpeed = 0;
        }
        else
        {
            
            if(!IsOriginal())
            {
                moveSpeed = 5f;
            }
        }

        if(IsOriginal())
        {
            moveSpeed = 0;
        }

        
    }

    private bool IsOriginal()
    {
        // Check if the name of the GameObject is exactly "Scamp"
        return gameObject.name == "Scamp";
    }

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
    

    IEnumerator WaitForSword()
    {
        yield return new WaitForSeconds(1f);

        isSwording = false;
    }


    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, LayerMask.GetMask("SolidObjects")) != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Use OnTriggerEnter2D for trigger events
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        

        
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isSwording = true;

            animator.SetTrigger("Shoot");

            if(!IsOriginal())
            {
                audioSource.PlayOneShot(swordSFX, 0.2f);
            }

            StartCoroutine(WaitForSword());
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {

            // Change speed to 0
            moveSpeed = 0f;

            // Start a coroutine to wait for the animation to complete
            StartCoroutine(WaitForAnimation());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isSwording = true;

            animator.SetTrigger("Shoot");

            StartCoroutine(WaitForSword());
        }
        
        
    }

    IEnumerator WaitForAnimation()
    {
        isDead = true;
        moveSpeed = 0f;

        // Trigger death animation
        animator.SetTrigger("DIEBITCH");

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

        // Wait for the length of the death animation
        yield return new WaitForSeconds(2f);

        // Notify the enemy wave manager about the death
        enemyWaveManager.counter--;

        //spawn item
        Instantiate(item, transform.position, Quaternion.identity);

        // Destroy the enemy gameObject after the animation is complete
        Destroy(gameObject);

        
    }
}
