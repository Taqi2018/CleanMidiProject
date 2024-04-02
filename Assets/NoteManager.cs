using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int noteNumber;
   

    public void SetNoteNumber(int noteNumber_)
    {
        noteNumber = noteNumber_;
    }
}
