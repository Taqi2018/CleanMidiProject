using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    // Start is called before the first frame update
    public float noteSize;
    public int numberOfTicks = 32;
    public GameObject noteBordar;
    public GameObject quatarNoteBordar;
    public int numberOfNotes;
    public float distanceBetweenNotes;
    public float zoomSpeed=10;
    float tempDis;
    int quartarNoteNumber = 0;

    GameObject NoteStart;
    private GameObject noteEnd;

    void Start()
    {
        instance = this;
        
        int screenWidth = Screen.width;
        tempDis = 0;
        int screenHeight = Screen.height;
        int xPos = 0;
/*        Debug.Log(screenHeight );*/
        /*Debug.Log(",");*/
     /*   Debug.Log(screenWidth);*/
      
     /*   var b=Instantiate(noteBordar,transform);
        b.transform.position = new Vector2(screenWidth / 2, screenHeight /2);*/
        for (int i = 0; i < numberOfNotes; i++)
        {

            var noteBordar = Instantiate(this.noteBordar, transform);


            noteBordar.transform.position = new Vector2(tempDis, screenHeight / 2);
            tempDis += distanceBetweenNotes;


            if (i > 0)
            {
                noteBordar.GetComponent<ZoomablePinch>().deltaMovement =(int)( (quartarNoteNumber + 1) * zoomSpeed);
            }
            if (i == 1)
            {
                noteBordar.GetComponent<ZoomablePinch>().deltaMovement = (int)((quartarNoteNumber + 1) * zoomSpeed);
                noteEnd = noteBordar;


      
            }
            if (i == 0)
            {
                noteBordar.GetComponent<ZoomablePinch>().deltaMovement = zoomSpeed;
                NoteStart = noteBordar;
            }
       


            // quatar Note
            float tempDisBetweenQnotes = 0;

            for (int j = 0; j < 4; j++)
            {
                quartarNoteNumber++;

                var q = Instantiate(quatarNoteBordar, transform);
                q.GetComponent<ZoomablePinch>().deltaMovement = ((quartarNoteNumber + 1)) * zoomSpeed;
                tempDisBetweenQnotes += distanceBetweenNotes / 4;
                q.transform.position = new Vector2(noteBordar.transform.position.x + tempDisBetweenQnotes, screenHeight / 2);
                if (j == 3)
                {
                    q.transform.GetChild(0).GetComponent<Image>().color = Color.black;
                }
       
            }
              
            

        }

        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
   
            distanceBetweenNotes = noteEnd.transform.position.x- NoteStart.transform.position.x;

/*            Debug.Log(distanceBetweenNotes.ToString() + "Distance Calc");*/
            /*            Debug.Log(noteEnd.transform.localPosition.x);
                        Debug.Log(noteEnd.transform.position.x);*/
        }

        if (Input.GetKeyUp(KeyCode.A))
        {

            distanceBetweenNotes = noteEnd.transform.position.x - NoteStart.transform.position.x;

        }
    }

/*    public int SnapByTick()
    {

        int distanceBetweenNote =(int) distanceBetweenNotes;
        int distanceMovedBasedOnTickValue = distanceBetweenNote / 1;

       
 
        return distanceMovedBasedOnTickValue;
    }*/
    public float GetPivotMoveMargin()
    {
        return NoteStart.transform.position.x;
    }

}
