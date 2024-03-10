using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public AudioClip fishingSFX;
    [SerializeField] private GameObject fishingLine;
    public GameObject fishPrefab;

    public PlayerController player;

    private int fishVar = 0;

    private float lower = 5f;
    private float higher = 10f;

    private bool isCastingLine = false;
    private bool canCast;

    private GameObject currentLine;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canCast && !isCastingLine)
        {
            // Interact function call
            StartCoroutine(CastLine());
        }

        // Check for player movement and cancel fishing if moving
        if (isCastingLine && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            StopFishing();
        }

        if(player.fishBoost && fishVar == 0)
        {

            lower = 1f;
            higher = 3f;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Player is touching water
            canCast = true;
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            // Player is touching water
            canCast = true;
        }
        
    }

    IEnumerator CastLine()
    {
        isCastingLine = true;

        // Play the shoot sound effect
        audioSource.PlayOneShot(fishingSFX, 0.3f);
        // Instantiate fishing line at player position
        currentLine = Instantiate(fishingLine, transform.position, Quaternion.identity);

        // Wait for a random time between 3 and 8 seconds
        yield return new WaitForSeconds(Random.Range(lower, higher));

        // Instantiate fish at the end of the line
        GameObject fish = Instantiate(fishPrefab, currentLine.transform.position, Quaternion.identity);

        // Cleanup: Destroy the line
        Destroy(currentLine);
        isCastingLine = false;
        canCast = false;
    }

    void StopFishing()
    {
        // Stop the fishing process and cleanup
        StopAllCoroutines();

        // Destroy the line if it exists
        if (currentLine != null)
        {
            Destroy(currentLine);
        }

        isCastingLine = false;
    }
}
