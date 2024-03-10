using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;

    [SerializeField] Text dialogueText;

    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialogue;
    public event Action OnHideDialogue;
   

    //expose Dialogue manager
    public static DialogueManager Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    Dialogue dialogue;

    //variable that shows current line in dialogue
    int currentLine = 0;

    //if the lines are currently in the middle of being displayed
    bool isTyping;

    //loads the lines that we use in dialogue
    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        //no returned value until end of frame
        yield return new WaitForEndOfFrame();

        //lets us use global variable of dialogue
        this.dialogue = dialogue;

        //changes state to dialogue
        OnShowDialogue?.Invoke();

        //Opens dialogue box
        dialogueBox.SetActive(true);

        //loads the current line
        StartCoroutine(TypeDialogue( dialogue.Lines[0] ) );

    }//end show dialogue

    public void HandleUpdate()
    {
        //check if input is z
        if( Input.GetKeyDown( KeyCode.Z ) && !isTyping )
        {
            //increment the line we are on
            ++currentLine;

            //if current Line is less that the total number if lines we need to show the Next line
            if( currentLine < dialogue.Lines.Count )
            {

                //We run that previously checked line of Dialogue
                StartCoroutine( TypeDialogue( dialogue.Lines[ currentLine ] ) );

            }//end if
            else //If the current line value exceeds the total we run this
            {

                //set dialogue box boolean to false so its off
                dialogueBox.SetActive( false );

                //so since the dialogue is finished we are resetting it to 0 for the next time we interact
                currentLine = 0;

                //turns off the dialogue box and changes the state back to free roam state
                OnHideDialogue?.Invoke();

            }//end else
        }
    }

    //Does dialogue letter by letter
    public IEnumerator TypeDialogue(string line)
    {

        //activate the is typing boolean to know we are typing
        isTyping = true;

        //we reset text to blank before typing
        dialogueText.text = "";

        //for loop of each letter in each line
        foreach ( var letter in line.ToCharArray() )
        {

            //add one letter to the dialogue per loop
            dialogueText.text += letter;

            //slight delay to allow letters to appear in sequence
            yield return new WaitForSeconds( 1f / lettersPerSecond );

        }//end for

        //reset the is Typing to false to allow end of dialogue
        isTyping = false;

    }//end Type Dialogue

}

