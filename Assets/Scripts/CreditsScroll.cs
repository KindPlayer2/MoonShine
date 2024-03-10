using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using TMPro;

public class CreditsScroll : MonoBehaviour
{
    public Vector3 startingPosition = new Vector3(190f, 129f, 0f);
    public Vector3 finishingPosition = new Vector3(190f, 9097f, 0f);

    public GameObject sysc;
    public float scrollSpeed = 1.0f;

    public Animator fader;

    private float elapsedTime = 0f;

    private int counter = 0;

    void Update()
    {
        // Increment the elapsed time based on the time since the last frame
        elapsedTime += Time.deltaTime;

        // Calculate the interpolation factor based on the elapsed time and scroll speed
        float t = elapsedTime * scrollSpeed;

        // Use Vector3.Lerp to move the GameObject smoothly between starting and finishing positions
        transform.position = Vector3.Lerp(new Vector3(1200f, 129f, 0f), new Vector3(1200f, 12000f, 0f), t);

        // If the interpolation factor exceeds 1, the movement is complete
        if (t >= 1.0f)
        {
            // Optionally, you can perform any actions after the movement is complete
            // For example, you might disable the script or trigger another event
            enabled = false;
        }

        if (counter == 0)
        {
            counter = 1;
            StartCoroutine(Fading());
        }
    }

    private IEnumerator Fading()
    {
        yield return new WaitForSeconds(40f);

        fader.SetTrigger("FadeOut");

        yield return new WaitForSeconds(5f);

        sysc.SetActive(true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(0);

    }
}
