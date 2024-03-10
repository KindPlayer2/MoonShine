using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distillery : MonoBehaviour, Interactable
{
    public PlayerController playa;
    public GameObject moonshinePrefab;
    private Animator animator;
    public GameObject uiImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Debug.Log("Distillery activated");

        if (playa.tychoCounter > 3)
        {

            StartCoroutine(Distil());
            
        }
        else
        {
            // Display UI image for 3 seconds
            StartCoroutine(DisplayUIImageForSeconds(3f));
        }
    }

    private IEnumerator DisplayUIImageForSeconds(float seconds)
    {
        // Activate UI image
        uiImage.SetActive(true);

        // Wait for specified seconds
        yield return new WaitForSeconds(seconds);

        // Deactivate UI image
        uiImage.SetActive(false);
    }

    private IEnumerator Distil()
    {
            // Trigger Distil animation
            animator.SetTrigger("Distil");

            yield return new WaitForSeconds(1f);

            // Subtract 3 from the counter
            playa.tychoCounter -= 3;

            // Calculate the spawn position for the moonshine object (x-5 from the distillery's position)
            Vector3 spawnPosition = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);


            // Instantiate moonshine object
            Instantiate(moonshinePrefab, spawnPosition, Quaternion.identity);
    }
}

