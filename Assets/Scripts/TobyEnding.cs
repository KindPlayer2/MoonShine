using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TobyEnding : MonoBehaviour, Interactable
{
    public SpriteRenderer playa;
    public SpriteRenderer tobey;
    public Transform ship;
    public Camera mainCamera; // Reference to the main camera

    public void Interact()
    {
        StartCoroutine(TobyTalk());
    }

    private IEnumerator TobyTalk()
    {
        yield return new WaitForSeconds(5f);

        playa.sortingLayerName = "Default";
        tobey.sortingLayerName = "Default";

        // Zoom out the camera
        StartCoroutine(ZoomOutCamera());

        // Move ship's Y value up by about 5 over 3 seconds
        float startY = ship.position.y;
        float targetY = startY + 5f;
        float elapsedTime = 0f;
        float duration = 3f;

        while (elapsedTime < duration)
        {
            ship.position = new Vector3(ship.position.x, Mathf.Lerp(startY, targetY, elapsedTime / duration), ship.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Quickly decrease ship's X value by about 50 over 1 second
        float startX = ship.position.x;
        float targetX = startX - 50f;
        elapsedTime = 0f;
        duration = 1f;

        while (elapsedTime < duration)
        {
            ship.position = new Vector3(Mathf.Lerp(startX, targetX, elapsedTime / duration), ship.position.y, ship.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        

        // Load the next scene
        SceneManager.LoadScene(3);
    }

    private IEnumerator ZoomOutCamera()
    {
        float startSize = mainCamera.orthographicSize; // Assuming mainCamera is an orthographic camera
        float targetSize = 30f; // Set the target size to 30
        float elapsedTime = 0f;
        float duration = 2f; // Adjust the duration of the zoom-out effect

        while (elapsedTime < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }



}
