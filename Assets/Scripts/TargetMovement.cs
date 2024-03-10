using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;

public class TargetMovement : MonoBehaviour
{

    public AudioClip explode;

    public Animator EarthAnimator;

    public GameObject credit;
    public Animator FadeAnimator;

    // Define the movement speed of the target
    public float moveSpeed = 5f;

    private AudioSource audioSource;

    // Define the range for triggering the "Boom" animation
    public float minX = 817f;
    public float maxX = 1022f;
    public float minY = 584f;
    public float maxY = 793f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Ensure this line is present
    }

    void Update()
    {
        // Move the target based on user input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Check if the space key is pressed and the target is within the specified range
        if (Input.GetKeyDown(KeyCode.Space) && IsWithinRange(transform.position))
        {
            EarthAnimator.SetTrigger("BOOM");
            audioSource.PlayOneShot(explode);
            StartCoroutine(WaitForFade());
        }
    }

    // Helper function to check if the target position is within the specified range
    private bool IsWithinRange(Vector3 position)
    {
        return (position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY);
    }

    private IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(1.6f);

        FadeAnimator.SetTrigger("FadeOut");

        credit.SetActive(true);
    }
}
