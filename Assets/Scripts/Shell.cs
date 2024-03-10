using UnityEngine;

public class Shell : MonoBehaviour
{
    public AudioClip explodeSFX;
    [SerializeField] private GameObject player;

    public PlayerController playa;

    private float speed = 0f;

    private Animator animator;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        // Check if the Shell's name is "Shell" and set the speed accordingly
        if (gameObject.name == "Shell")
        {
            speed = 0f;
        }
        else
        {
            speed = 3f;
        }
    }

    private void Update()
    {
        // Rotate towards the player
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collision is with the player
        if (other.gameObject == player)
        {
            // Trigger KABOOM animation
            animator.SetTrigger("KABOOM");

            if(gameObject.name != "Shell" && !playa.isInside)
            {
                // Play the shoot sound effect
                audioSource.PlayOneShot(explodeSFX, 0.1f);
            }

            // Wait for 1 second
            StartCoroutine(WaitAndDestroy());
        }
    }

    private System.Collections.IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(5f);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}
