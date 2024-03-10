using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingText : MonoBehaviour
{
    public float scrollSpeed = 150;
    public Vector2 targetPosition = new Vector2(957f, 4176f);
    public float stopTime = 2f;

    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            // Move the text block upwards
            transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

            // Check if the target position is reached
            if (transform.position.y >= targetPosition.y)
            {
                // Stop moving
                isMoving = false;

                // Freeze in place for stopTime seconds
                Invoke("LoadScene", stopTime);
            }
        }
    }

    void LoadScene()
    {
        // Load Scene 1
        SceneManager.LoadScene(1);
    }
}
