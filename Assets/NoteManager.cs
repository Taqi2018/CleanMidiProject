using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int noteNumber;
    public static EventHandler <OnNoteTriggerEventArgs>OnNoteTrigger;
    public  class OnNoteTriggerEventArgs: EventArgs
    {
        public int noteNumber;
    }
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

   

    public int GetNoteNumber()
    {
        return noteNumber;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name + "Strike with me");
        OnNoteTrigger?.Invoke(this, new OnNoteTriggerEventArgs { noteNumber = noteNumber });
    }


}
