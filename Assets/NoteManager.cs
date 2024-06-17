using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NoteManager : MonoBehaviour
{
     // Start is called before the first frame update
     [SerializeField] int noteNumber;
     [SerializeField] public int noteDuration;
     [SerializeField] public int noteTickValue;
     public float Width;
     public float Anchor;
     public static EventHandler<OnNoteTriggerEventArgs> OnNoteTrigger;

    public float noteSpeed;

    public float distanceMoved;
    private float currentNoteXScale;
    private float noteXScale;

    void Start()
    {
        distanceMoved = 0;
        currentNoteXScale = transform.localScale.x;
    }
    public class OnNoteTriggerEventArgs : EventArgs
     {
          public int noteNumber;
        public int noteDuration;

     }

     void Awake()
     {
          Width = 0;
          Anchor = 0;
          noteDuration = 0;

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
          OnNoteTrigger?.Invoke(this, new OnNoteTriggerEventArgs { noteNumber = noteNumber,
              noteDuration=noteDuration });

       
     }

     public void SetWidth(float w)
     {
          Width = w;

     }
     public float GetWidth()
     {
          return Width;
     }

     public void SetAnchor(float a)
     {
          Anchor = a;

     }
     public float GetAnchor()
     {
          return Width;
     }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(noteSpeed * Time.deltaTime, 0, 0);

/*            noteXScale = ((GridManager.instance.distanceBetweenNotes / 2) * 0.67f) / 150f;

            float deltaX = noteXScale - currentNoteXScale;

            Debug.Log(GridManager.instance.distanceBetweenNotes.ToString() + " Change in x ");
            transform.localScale += new Vector3(deltaX, 0, 0);
            currentNoteXScale = noteXScale;*/
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(noteSpeed * Time.deltaTime, 0, 0);
            distanceMoved += noteSpeed;
/*
            noteXScale = ((GridManager.instance.distanceBetweenNotes / 2) * 0.67f) / 150f;
            
            float deltaX = noteXScale - currentNoteXScale;

    
            transform.localScale+= new Vector3(deltaX, 0, 0);
            currentNoteXScale = noteXScale;*/
        }
        //0.67-->150  :x-->200
        //150/0.67  =200/x
        //150/0.67  =200/x
        //150/0.67  =200/x
        //150/0.67  =200/x
        //x=200*0.67/150

        // Calculate the note scale every frame
        noteXScale = ((GridManager.instance.distanceBetweenNotes / 2) * 0.67f) / 150f;

        transform.localScale = new Vector3(noteXScale, 1, 1);
    }
}
