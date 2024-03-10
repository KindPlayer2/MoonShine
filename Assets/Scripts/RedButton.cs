using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class RedButton : MonoBehaviour, Interactable
{
    // Start is called before the first frame update
    public void Interact()
    {
        StartCoroutine(ButtonPress());
    }

    private IEnumerator ButtonPress()
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(2);
    }
}
