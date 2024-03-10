using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum is array that can have different states
public enum GameState {FreeRoam, Dialogue, Battle}

public class GameController : MonoBehaviour
{
    //adds player controller field within unity inspector
    [SerializeField] PlayerController playerController;

    //Just defines what state of the game we are in, to do with dialogue and fighting.
    GameState state;

    //Runs once at the start
    private void Start()
    {
        //Function to Enter Dialogue state
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };//end dialogue function

        //function to return to Freeroam mode after Dialogue
        DialogueManager.Instance.OnHideDialogue += () =>
        {
            //Condition to see if we are in dialogue before we return to freeroam
            if(state == GameState.Dialogue)
            {
                state = GameState.FreeRoam;
            }//end if
        
        };//end return to FreeRoam functio
    }//end Start

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();

        }
        else if( state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        else if( state == GameState.Battle)
        {

        }
    }
}
