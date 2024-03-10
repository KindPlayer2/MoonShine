using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public AudioClip explodeSFX;
    private Animator animator;
    private AudioSource audioSource;

    public PlayerController playa;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet"))
        {

            StartCoroutine(Explode());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
        {

            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        if(gameObject.name != "Landmine" && !playa.isInside)
        {
            // Play the shoot sound effect
            audioSource.PlayOneShot(explodeSFX, 0.1f);
        }

        animator.SetTrigger("Explode");

        yield return new WaitForSeconds(0.8f);

        // Destroy the enemy gameObject after the animation is complete
        Destroy(gameObject);

    }
    
}
