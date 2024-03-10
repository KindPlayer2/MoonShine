using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;



public class PlayerController : MonoBehaviour
{
    public GameObject houseSong;
    public GameObject caveSong;
    public GameObject outsideSong;
    public GameObject prsZ;
    public GameObject prsF;

    private int Zcount = 0;
    private int Fcount = 0;

    public AudioClip shootSFX;
    public AudioClip doorSFX;
    public AudioClip getItem;
    public AudioClip getTycho;
    public AudioClip reloadSFX;
    public AudioClip emptySFX;
    public AudioClip warpSFX;
    public AudioClip blockSFX;

    private AudioSource audioSource;

    [SerializeField] private TMP_Text starText;
    [SerializeField] private TMP_Text tychoText;
    [SerializeField] private TMP_Text scrapText;
    [SerializeField] private TMP_Text RobotHeadText;
    [SerializeField] private TMP_Text canonText;
    [SerializeField] private TMP_Text fishText;
    [SerializeField] private TMP_Text rodText;
    [SerializeField] private TMP_Text moonshineText;
    [SerializeField] private TMP_Text waveText;

    public int waveCounter = 0;

    public bool speedBoost = false;
    private int speedBoostVar = 0;

    public bool fishBoost = false;

    public bool healthBoost = false;

    public bool warpBoost = false;
    private int warpBoostVar = 0;

    public bool reloadBoost = false;
    private int reloadBoostVar = 0;

    private float reloadTemp = 2f;

    public bool itemBoost = false;

    private bool Invulnerable = false;

    public int starCounter = 0;
    public int tychoCounter = 0;
    public int scrapCounter = 0;
    public int RobotHeadCounter = 0;
    public int canonCounter = 0;
    public int fishCounter = 0;
    public int rodCounter = 1;
    public int moonshineCounter = 0;

    //Length of bullet
    [SerializeField] float bulletLife;
    [SerializeField] private RectTransform inventoryRectTransform;
    [SerializeField] private RectTransform upgradeRectTransform;
    [SerializeField] private RectTransform WaveRectTransform;


    private bool isInventoryOpen = false;
    private Vector2 openPosition = new Vector2(710f, 100f);
    private Vector2 closedPosition = new Vector2(-420f, 100f);

    private Vector2 upgradeOpenPosition = new Vector2(300f, -100f);
    private Vector2 upgradeClosedPosition = new Vector2(1440f, -100f);

    private Vector2 WaveOpenPosition = new Vector2(-1070f, -100f);
    private Vector2 WaveClosedPosition = new Vector2(-1358f, -100f);

    private float InvmoveSpeed = 5f; // Adjust the speed as needed

    private bool isInventory = false;

    public bool isInside = true;

    public float newPositionX;

    public float newPositionY;

    //Speed of player
    public float moveSpeed;

    public GameObject greenFilter;

    //check if we are moving
    private bool isMoving;

    //user input stored as coordinate (x,y)
    private Vector2 input;

    //Create an animator controller
    private Animator animator;

    //layer of things we bump into
    public LayerMask solidObjectsLayer;

    private float reloadTimer = 3f;

    //things we talk to
    public LayerMask interactablesLayer;

    // Prefab for the bullet
    public GameObject bulletPrefab;

    // Reference to the current shooting animation direction
    private Vector2 shootingDirection;

    // Previous shooting direction
    private Vector2 previousShootingDirection;

     // Health variables
    private int maxHealth = 8;
    public int health;

    // UI Animator for health
    [SerializeField] private Animator healthAnimator;

    // UI Animator for Weapon
    [SerializeField] private Animator gunAnimator;

    //UI for warp
    [SerializeField] private Animator WarpUI;


    // UI Animator for Fade out
    [SerializeField] private Animator fade;

    // Flag to check if the player is currently dashing
    private bool isDashing = false;

    // Dash cooldown
    private float dashCooldown = 8f;

    // Flag to check if the dash is on cooldown
    private bool isDashCooldown = false;

    //boolean to see if can shoot
    //private bool canShoot = true;

    //boolean to see if we are blocking or not
    private bool blocking = false;

    //ammo
    [SerializeField] int ammo = 6;

    //Are we currently relaoding our gun
    private bool reloading = false;

    //Death message
    [SerializeField] Dialogue dialogue;

    private bool dooring = false;
    

    //Runs at the start of the program
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Ensure this line is present
        // Set the default shooting direction to down when the player starts
        previousShootingDirection = Vector2.down;
        // Initialize health
        health = maxHealth;
    }

    private void Start()
    {
        //fade.SetTrigger("FadeIn");
    }

    //every frame
    public void HandleUpdate()
    {

        if (!isMoving && !isDashing && !blocking && !dooring)
        {
            // Determine direction from input
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if(Zcount == 0)
            {
                Zcount = 1;
                
                StartCoroutine(showDisplayZ());
            }

            if (input != Vector2.zero)
            {
                // If there is new input, set the shooting direction
                shootingDirection = input.normalized;
            }

            // set MoveX and Y values with correct directions for animations
            animator.SetFloat("moveX", shootingDirection.x);
            animator.SetFloat("moveY", shootingDirection.y);

            if (input != Vector2.zero)
            {
                // use input to determine the target position
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    // call Move as a Coroutine with the target position
                    StartCoroutine(Move(targetPos));
                }
            }

        }

        // Set the animation boolean to show we are moving to activate walk animation
        animator.SetBool("isMoving", isMoving);

        waveText.text = "wave: " + waveCounter.ToString();

        starText.text = starCounter.ToString();
        tychoText.text = tychoCounter.ToString();
        scrapText.text = scrapCounter.ToString();
        RobotHeadText.text = RobotHeadCounter.ToString();
        canonText.text = canonCounter.ToString();
        fishText.text = fishCounter.ToString();
        rodText.text = rodCounter.ToString();
        moonshineText.text = moonshineCounter.ToString();

        // Press Z to interact with NPC
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Interact function call
            Interact();
        }

        //fixes glitch where eating fish gives more than maximum health, which leads to further problems
        if(health > maxHealth)
        {
            health = maxHealth;
        }


        // Handle shooting
        if (Input.GetKeyDown(KeyCode.Space) && !blocking)
        {
            if(ammo != 0 && !reloading)
            {
                // Set the previous shooting direction
                previousShootingDirection = shootingDirection;

                // Trigger the shooting animation
                animator.SetTrigger("shoot");

                // Spawn a bullet
                SpawnBullet();

                // Play the shoot sound effect
                audioSource.PlayOneShot(shootSFX, 0.1f);

                //ammo is one less
                ammo--;

                //Lower the ammo UI to -1
                gunAnimator.SetTrigger("ShotBullet");
            }
            if(ammo == 0)
            {
                // Play the shoot sound effect
                audioSource.PlayOneShot(emptySFX, 0.1f);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if(fishCounter > 0)
            {
                fishCounter--;

                healthAnimator.SetTrigger("Fish");

                health = health + 3;
            }
        }


        // Handle shooting
        if (Input.GetKeyDown(KeyCode.B) && !blocking)
        {
            
            //call block co routing
            StartCoroutine(Block());            

        }

        // Dash when left control key is pressed and not currently dashing
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing && !isDashCooldown && !isMoving && !blocking)
        {
            // Trigger the Warp animation
            animator.SetTrigger("Dash"); 

            // Play the warp sound effect
            audioSource.PlayOneShot( warpSFX , 0.1f ); 

            StartCoroutine(Dash()); 
              
        }

        // Handle Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            
            //call Reload function
            StartCoroutine(Reload());            

        }

        //Drink MoonShine
        if (Input.GetKeyDown(KeyCode.Q))
        {

            //check if we have any moonshine in inventory
            if( moonshineCounter > 0 )
            {
                //activate the effect
                StartCoroutine(moonShineEffect());
            }//end if
            
        }//end if

        //speed boost upgrade is purchased, we update the speed a bit
        if(speedBoost && speedBoostVar == 0)
        {
            speedBoostVar = 1;

            moveSpeed = moveSpeed * 1.5f;
        }

        //warp boost upgrade is purchased, we update the warp timer
        if(warpBoost && warpBoostVar == 0)
        {
            warpBoostVar = 1;

            dashCooldown = 4f;

            WarpUI.speed = WarpUI.speed * 2;
        }

        //speed boost upgrade is purchased, we update the speed a bit
        if(reloadBoost && reloadBoostVar == 0)
        {
            reloadBoostVar = 1;

            reloadTimer = 1f;
            reloadTemp = 1f;
        }

        Debug.Log(health);


        
    }//end Handle Update

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if(!isInventory)
        {
            if (isInventoryOpen)
            {
                // Set the Inventory UI to active
                inventoryRectTransform.gameObject.SetActive(true);

                //set the upgrade UI to active
                upgradeRectTransform.gameObject.SetActive(true);


                // Start moving the UI towards the open position
                StartCoroutine(MoveInventory(openPosition));

                StartCoroutine(MoveWave(WaveOpenPosition));

                // Start moving the UI towards the open position
                StartCoroutine(MoveUpgrade(upgradeOpenPosition));
            }
            else
            {
                // Start moving the UI towards the closed position
                StartCoroutine(MoveInventory(closedPosition));

                StartCoroutine(MoveWave(WaveClosedPosition));

                // Start moving the UI towards the closed position
                StartCoroutine(MoveUpgrade(upgradeClosedPosition));
            }
        }

        
    }

    private IEnumerator showDisplayZ()
    {
        yield return new WaitForSeconds(2f);
        prsZ.SetActive(true);
        yield return new WaitForSeconds(4f);
        prsZ.SetActive(false);
    }

    System.Collections.IEnumerator MoveUpgrade(Vector2 targetPosition)
    {
        isInventory = true;
        while (Vector2.Distance(upgradeRectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            // Smoothly move the UI towards the target position
            upgradeRectTransform.anchoredPosition = Vector2.Lerp(
                upgradeRectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * InvmoveSpeed
            );

            yield return null;
        }

        // Ensure the UI reaches the exact target position
        upgradeRectTransform.anchoredPosition = targetPosition;

        // If the Inventory is closed, set it to inactive after moving
        if (!isInventoryOpen)
        {
            upgradeRectTransform.gameObject.SetActive(false);
        }
        isInventory = false;
    }

    System.Collections.IEnumerator MoveInventory(Vector2 targetPosition)
    {
        isInventory = true;
        while (Vector2.Distance(inventoryRectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            // Smoothly move the UI towards the target position
            inventoryRectTransform.anchoredPosition = Vector2.Lerp(
                inventoryRectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * InvmoveSpeed
            );

            yield return null;
        }

        // Ensure the UI reaches the exact target position
        inventoryRectTransform.anchoredPosition = targetPosition;

        // If the Inventory is closed, set it to inactive after moving
        if (!isInventoryOpen)
        {
            inventoryRectTransform.gameObject.SetActive(false);
        }
        isInventory = false;
    }

    System.Collections.IEnumerator MoveWave(Vector2 targetPosition)
    {
        isInventory = true;
        while (Vector2.Distance(WaveRectTransform.anchoredPosition, targetPosition) > 0.1f)
        {
            // Smoothly move the UI towards the target position
            WaveRectTransform.anchoredPosition = Vector2.Lerp(
                WaveRectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * InvmoveSpeed
            );

            yield return null;
        }

        // Ensure the UI reaches the exact target position
        WaveRectTransform.anchoredPosition = targetPosition;

        isInventory = false;
    }

    //function to reload revolver
    IEnumerator Reload()
    {

        //we are now reloading
        reloading = true;

        //it takes a few seconds to reload

        // Play the warp sound effect
        audioSource.PlayOneShot(reloadSFX, 0.2f);

        yield return new WaitForSeconds(reloadTimer);

        //reload the gun on the UI
        gunAnimator.SetTrigger("Reload");

        //ammo is full again
        ammo = 6;

        //no longer reloading
        reloading = false;

    }//end reload

    //stops all inputs and stops taking damage, only lasts one second
    IEnumerator Block()
    {

        //stops anything else from being done
        blocking = true;

        //do the block animation
        animator.SetTrigger("Block");

        // Play the warp sound effect
        audioSource.PlayOneShot(blockSFX,0.1f);

        //1 second timer of block animation
        yield return new WaitForSeconds(2f);

        //allow everything to happen again
        blocking = false;

    }//end Block

    IEnumerator Dash()
    {
        // Set the dashing flag to true
        isDashing = true;

        // Wait for the dash cooldown duration
        yield return new WaitForSeconds(1.6f);

        // Calculate the dash distance based on your desired value
        float dashDistance = 8.0f;

        // Calculate the target position for the dash
        Vector3 dashTarget = transform.position + new Vector3(shootingDirection.x, shootingDirection.y, 0f) * dashDistance;

        // Check if the dash target is walkable
        if (IsWalkable(dashTarget))
        {
            // While the player is dashing and the dash target is walkable
            while ((dashTarget - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                // Move towards the dash target at a higher speed
                transform.position = Vector3.MoveTowards(transform.position, dashTarget, (moveSpeed * 7) * Time.deltaTime);
                yield return null;
            }
        }

        // Trigger the end warp animation
        animator.SetTrigger("Unwarp");

        

        // Player has finished dashing
        isDashing = false;

        // Start the dash cooldown
        StartCoroutine(DashCooldown());
    }


    IEnumerator DashCooldown()
    {
        // Set dash cooldown to be true
        isDashCooldown = true;

        //Ui for warp refill
        WarpUI.SetTrigger("Warp");

        // Wait for the dash cooldown duration
        yield return new WaitForSeconds(dashCooldown);

        // Set dash cooldown to be false
        isDashCooldown = false;
    }

    void Damage()
    {
        if(!isDashing && !blocking && !Invulnerable)
        {
            // Reduce health by 1
            health--;

            // Trigger the HPlowered animation in the UI Animator
            healthAnimator.SetTrigger("HPlowered");

            StartCoroutine(tookHit());

            // Check for death condition
            if (health <= 0)
            {
                Death();
            }  
        }
        
    }

    IEnumerator tookHit()
    {
        Invulnerable = true;

        yield return new WaitForSeconds(0.8f);

        Invulnerable = false;
    }

    void Death()
    {

        //play death animation
        animator.SetTrigger("Death");

        //Fade to black after you die :( hope fully you never get this far
        fade.SetTrigger("FadeOut");

        //Back to main menu
        StartCoroutine(BackToMenu());
        
    
        
    }

    IEnumerator BackToMenu()
    {
        //wait for screen to be completely dark
        yield return new WaitForSeconds(3f);

        //got to main menu
        SceneManager.LoadScene(4);

    }

    IEnumerator WaitWhat()
    {
        dooring = true;
        yield return new WaitForSeconds(0.5f);
        // Play the shoot sound effect
        audioSource.PlayOneShot(doorSFX, 0.1f);
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
        dooring = false; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        // Check if triggered by an object tagged as "EnemyAttack"
        if (other.gameObject.CompareTag("EnemyAttack"))
        {

            // Call the Damage function
            Damage();
        }

        if (other.gameObject.CompareTag("Scrap"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                scrapCounter++;
                scrapCounter++;
                
            }
            else
            {
                scrapCounter++;
            }
            

            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);
            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("Tychonite"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                tychoCounter++;
                tychoCounter++;
            }
            else
            {
                tychoCounter++;
            }

            // Play the get item sound effect
            audioSource.PlayOneShot(getTycho, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("Star"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                starCounter++;
                starCounter++;
            }
            else
            {
                starCounter++;
            }
            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("rboHead"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                RobotHeadCounter++;
                RobotHeadCounter++;
            }
            else
            {
                RobotHeadCounter++;
            }

            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("moonshine"))
        {

            // Call the Damage function
            moonshineCounter++;

            
            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("rod"))
        {

            // Call the Damage function
            rodCounter++;

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("Fish"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                fishCounter++;
                fishCounter++;
            }
            else
            {
                fishCounter++;
            }

            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("canon"))
        {

            // Call the Damage function
            if(itemBoost)
            {
                canonCounter++;
                canonCounter++;
            }
            else
            {
                canonCounter++;
            }

            // Play the get item sound effect
            audioSource.PlayOneShot(getItem, 0.1f);

            // Destroy the star GameObject
            Destroy(other.gameObject);
            
        }
        

        // Check if triggered by an interior Door"
        if (other.gameObject.CompareTag("Door"))
        {   
            newPositionX = -158f;
            newPositionY = -12f;

            houseSong.SetActive(false);
            outsideSong.SetActive(true);

        
           StartCoroutine(WaitWhat());
           isInside = false;
        }
        

        // Check if triggered by an interior Door"
        if (other.gameObject.CompareTag("DoorEx"))
        {

            newPositionX = -372f;
            newPositionY = -20f;
            isInside = true;

            houseSong.SetActive(true);

            outsideSong.SetActive(false);


            StartCoroutine(WaitWhat());
           
        }
        

        if (other.gameObject.CompareTag("CaveEntrance"))
        {

            if(Fcount == 0)
            {
                Fcount = 1;
                StartCoroutine(displayF());
            }

            caveSong.SetActive(true);
            
            outsideSong.SetActive(false);

            newPositionX = -382f;
            newPositionY = -64f;

            StartCoroutine(WaitWhat());
            isInside = true;
           
        }
        

        if (other.gameObject.CompareTag("Cave"))
        {

            newPositionX = -136f;
            newPositionY = -2f;

            caveSong.SetActive(false);
            outsideSong.SetActive(true);

            StartCoroutine(WaitWhat());
            isInside = false;
           
        }
        
    }

    private IEnumerator displayF()
    {
        yield return new WaitForSeconds(1f);

        prsF.SetActive(true);

        yield return new WaitForSeconds(3f);

        prsF.SetActive(false);

    }

    //code to give the effect of drinking moonshine
    private IEnumerator moonShineEffect()
    {
        //add the green filter
        greenFilter.SetActive(true);

        //go faster
        moveSpeed = moveSpeed * 1.5f;

        //infinite reload
        reloadTimer = 0f;

        //Invulnerable
        Invulnerable = true;
        
        //lasts for 10 seconds
        yield return new WaitForSeconds(10f);

        //lose the filter
        greenFilter.SetActive(false);

        //reset the timer to normal
        reloadTimer = reloadTemp;

        //Vulnerable again
        Invulnerable = false;

        //reset move speed 
        moveSpeed = moveSpeed / 1.5f;
    }



    // Interact function, used to interact with NPCs
    void Interact()
    {
        // Determine the direction our character is facing
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));

        // Add the direction to our current position to get the position of what we interact with
        var interactPos = transform.position + facingDir;

        // Create the collider for interactablesLayer objects
        var collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactablesLayer);

        // Check if we are colliding with an NPC in the interactables Layer
        if (collider != null)
        {

            // Get the collider and the component, if it is interactable, run Interact() on the NPC's script
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    // Movement function
    IEnumerator Move(Vector3 targetPos)
    {
        // Set moving to be true
        isMoving = true;

        // While the button is being pressed
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Current position is changed to be the target position at the given speed
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;


        }
        // Current position is changed to target position
        transform.position = targetPos;

        // We are no longer moving
        isMoving = false;
    }

    // Boolean to see if we are colliding with the solid object layer or interactables layer
    private bool IsWalkable(Vector3 targetPos)
    {
        // If collision detected, no walk allowed, if not detected, you may continue. Happy days!
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactablesLayer) != null)
        {
            // If we are touching the wall, this is returned
            return false;
        }
        else
        {
            // Otherwise, we return this
            return true;
        }
    }

    // Function to spawn a bullet
    private void SpawnBullet()
    {
        // Calculate the position for the bullet based on the shooting direction
        Vector3 bulletSpawnPosition = transform.position + new Vector3(shootingDirection.x, shootingDirection.y, 0f);

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPosition, Quaternion.identity);

        // Access the bullet script (assuming you have one) to set its direction
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(previousShootingDirection);
        }
    }
}
