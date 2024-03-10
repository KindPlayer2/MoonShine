using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInterior : MonoBehaviour
{
    [SerializeField] PlayerController playerPosition;
    [SerializeField] Transform player;
    public void Interact()
    {
        
       player.transform.position = new Vector3(-158, -11, transform.position.z);
    }
}
