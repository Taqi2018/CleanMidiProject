using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int noteNumber;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNoteNumber(int noteNumber_)
    {
        noteNumber = noteNumber_;
    }
}
