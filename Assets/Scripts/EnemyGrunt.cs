using System.Collections;
using UnityEngine;

public class EnemyGrunt : MonoBehaviour
{
    private bool isDead = false;

    public AudioClip shootSFX;
    public GameObject item;
    // UI Animator for health
    [SerializeField] private Animator healthAnimator;
    public PlayerController playa;
    public Transform playerPosition;
    public float moveSpeed = 5f;
    public float stopDistance = 10f;
    public float waitTime = 1f;
    public GameObject bulletPrefab; // Serialized field for the enemy bullet prefab
    public float shootCooldown = 3f; // Cooldown time for shooting
    private Animator animator;
    private bool isMoving = false;
    private Vector3 shootingDirection;
    private bool canShoot = true;

    [SerializeField] EnemyWaveManager enemyWaveManager;

    private bool isShooting = false;
    
    //Checks if the enemy is in range to shoot
    private bool inPosition = false;

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

    }

    void Update()
    {
        if(isDead)
        {
            moveSpeed = 0;
        }
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        //Debug.Log(distanceToPlayer);

        
        //Check if we are in position before we shoot
        if (!inPosition && !playa.isInside  && !isDead)
        {

            //go to player NOW
            MoveTowardsPlayer();

        }//end if 
        else //we are in position FIRE IN THE HOLE
        {
        
            //shoot the player
            StartCoroutine(StopAndShoot()); 

        }//end else

        if(playa.isInside)
        {
            moveSpeed = 0;
        }
        else
        {
            moveSpeed = 2f;
        }

        if(IsOriginal())
        {
            moveSpeed = 0;
        }

        
    }

    private bool IsOriginal()
    {
        // Check if the name of the GameObject is exactly "Grunt"
        return gameObject.name == "Grunt";
    }

    private void MoveTowardsPlayer()
    {
        if (!isMoving && !isShooting)
        {
            // Calculate direction towards the player
            Vector3 direction = (playerPosition.position - transform.position).normalized;

            // Use the calculated direction for animations
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            // Calculate potential target positions
            Vector3 playerPos = playerPosition.position;
            Vector3[] potentialPositions = new Vector3[]
            {
                playerPos + Vector3.left * 5f,
                playerPos + Vector3.right * 5f,
                playerPos + Vector3.up * 5f,
                playerPos + Vector3.down * 5f,
                playerPos + (Vector3.left + Vector3.up).normalized * 5f,
                playerPos + (Vector3.right + Vector3.up).normalized * 5f,
                playerPos + (Vector3.left + Vector3.down).normalized * 5f,
                playerPos + (Vector3.right + Vector3.down).normalized * 5f
            };

            // Find the closest potential position
            Vector3 closestPosition = potentialPositions[0];
            float closestDistance = Vector3.Distance(transform.position, closestPosition);

            foreach (Vector3 position in potentialPositions)
            {
                float distance = Vector3.Distance(transform.position, position);
                if (distance < closestDistance)
                {
                    closestPosition = position;
                    closestDistance = distance;
                }
            }

            // Move towards the closest position
            if (IsWalkable(closestPosition))
            {
                StartCoroutine(Move(closestPosition));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
        inPosition = true;
    }

    IEnumerator StopAndShoot()
    {
        isShooting = true;

        if (canShoot)
        {
            canShoot = false;


            for (int i = 0; i < 3; i++)
            {
                //shootingDirection = (playerPosition.position - transform.position).normalized;
                if(gameObject.name != "Grunt" && !playa.isInside)
            {
               // Play the shoot sound effect
                audioSource.PlayOneShot(shootSFX, 0.1f); 
            }

                animator.SetTrigger("Shoot");

                SpawnBullet();

                yield return new WaitForSeconds(0.5f);
            }

            // Reset the shoot cooldown after shooting
            yield return new WaitForSeconds(shootCooldown);
            canShoot = true;
        }

        isShooting = false;
        inPosition = false;
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

    private void SpawnBullet()
    {
        // Use the current facing direction for shooting
        Vector2 bulletDirection = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY")).normalized;

        // Calculate the position for the bullet based on the shooting direction
        Vector3 bulletSpawnPosition = transform.position + new Vector3(bulletDirection.x, bulletDirection.y, 0f);

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);

        // Access the bullet script (assuming you have one) to set its direction
        EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
        if (enemyBulletScript != null)
        {
            enemyBulletScript.SetDirection(bulletDirection);
        }
    }


    // Use OnTriggerEnter2D for trigger events
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {

            // Change speed to 0
            moveSpeed = 0f;

            // Start a coroutine to wait for the animation to complete
            StartCoroutine(WaitForAnimation());
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
