using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] List<string> lines;

    //function to get lines from other methods
    public List<string> Lines
    {
        get { return lines; }
    }
}
