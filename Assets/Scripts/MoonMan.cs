using UnityEngine;

public class MoonMan : MonoBehaviour
{
    public float walkingSpeed = 3f; // Adjust the walking speed as needed
    private bool isFacingRight = true;
    private Animator animator;
    private float timer = 0f;
    private float walkTime = 2f; // Adjust the time for each direction as needed

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on MoonMan GameObject.");
        }

        // Set the initial value of IsWalking parameter to true
        animator.SetBool("IsWalking", true);
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to flip direction
        if (timer >= walkTime)
        {
            FlipMoonMan();
            timer = 0f; // Reset the timer
        }

        // Move MoonMan to the right or left
        float direction = isFacingRight ? 1f : -1f;
        transform.Translate(Vector2.right * direction * walkingSpeed * Time.deltaTime);
    }

    private void FlipMoonMan()
    {
        // Flip the sprite horizontally
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Trigger the walking animation
        animator.SetBool("IsWalking", !animator.GetBool("IsWalking"));
    }
}
