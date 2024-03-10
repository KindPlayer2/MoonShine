using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the default AudioClip for hover sound
        audioSource.clip = hoverSound;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover sound when the mouse enters the button
        audioSource.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Play click sound when the button is clicked
        audioSource.clip = clickSound;
        audioSource.Play();

        // Optionally, you can handle additional button click logic here
    }
}
