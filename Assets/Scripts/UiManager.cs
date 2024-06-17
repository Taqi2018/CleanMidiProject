using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class UiManager : MonoBehaviour
{

     private GraphicRaycaster rayCaster;
     public Canvas canvas;
     List<RaycastResult> raycastOutput;
     public static EventHandler<OnTrackTriggerEventArgs> OnPlaceNoteTrigger;
     private bool isNoteOnTap;
     private bool isTapStarted;
     private Transform selectedNote;
     private float previousPosition;
     private Transform toBeStretchedNoteLeft, toBeStretchedNoteRight;
     private float leftSide;
     private float noteEndAtCurrentPos;
     private bool isStretching;
    private float startNoteDistance;
    public float clickMargin = 2f;
     private Vector3 lastMousePosition;
     public float snapValue;
     [SerializeField] int howManyTicksPerUnitBasedOnSnapValue;
     float objectWidth;
     public float snapDistance;
     int ifGreater;
     private float newWidth;
     private float newAnchor;
     private int i;
     public Text NoteTickValueText;
     public Text NoteDurationValueText;
    private int counter;
    private bool isLock;


    public static UiManager instance;
    public class OnTrackTriggerEventArgs : EventArgs
     {
          public TrackManager TrackSelected;
          public Vector2 NotePlacePosition;
          public int InitialNoteTickValue;
        public int noteBordar;
     }

     private void OnEnable()
     {
          InputManager.OnTapEndInput += UiChangeOnTapEndTrigger;
          InputManager.OnTapStartInput += UiChangeOnTapStartTrigger;
     }

     void Start()
     {
        instance = this;
          rayCaster = canvas.GetComponent<GraphicRaycaster>();
          raycastOutput = new List<RaycastResult>();
          isStretching = false;
        startNoteDistance = 300;

    }

     private void UiChangeOnTapStartTrigger(object sender, InputManager.TapInputEventArgs e)
     {
          howManyTicksPerUnitBasedOnSnapValue = HowManyTicksPerUnitBasedOnSnapValue(snapValue);
          StretchNoteIfPossible(e);
          if (!isStretching)
          {
               MoveNoteIfPossible(e);
          }
     }

     private void MoveNoteIfPossible(InputManager.TapInputEventArgs e)
     {
          PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());
          mPointerEventData.position = e.TouchPosition;
          rayCaster.Raycast(mPointerEventData, raycastOutput);
          
          foreach (RaycastResult r in raycastOutput)
          {
               if (r.gameObject.transform.parent.TryGetComponent<NoteManager>(out NoteManager noteManager))
               {
                    noteManager.transform.SetParent(canvas.transform);
                    selectedNote = noteManager.transform;
                    previousPosition = selectedNote.transform.position.x;
                    NoteTickValueText.text = noteManager.noteTickValue.ToString();
                  //  NoteDurationValueText.text = noteManager.noteDuration.ToString();
               }
          }
          raycastOutput.Clear();
     }

     private void StretchNoteIfPossible(InputManager.TapInputEventArgs e)
     {
          PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
          pointerEventData.position = e.TouchPosition;
          rayCaster.Raycast(pointerEventData, raycastOutput);

          foreach (RaycastResult r in raycastOutput)
          {
               if (r.gameObject.transform.parent.TryGetComponent<NoteManager>(out NoteManager noteManager))
               {
                    BoxCollider2D noteCollider = noteManager.transform.GetChild(0).GetComponent<BoxCollider2D>();

                
                    noteEndAtCurrentPos = noteManager.transform.position.x + noteCollider.bounds.size.x - (noteCollider.bounds.size.x/100)*20;

                    if (e.TouchPosition.x >= noteEndAtCurrentPos)
                    {
                         toBeStretchedNoteRight = noteManager.transform;
                      
                         selectedNote = null;
                         isStretching = true;
                         lastMousePosition.x = e.TouchPosition.x;
                    }

               }
          }
          raycastOutput.Clear();
     }
    public void NoteMovement()
    {
        if (selectedNote != null)
        {
            Vector3 newPosition = InputManager.instance.GetTapPosition();


            newPosition.x = HowManySnapUnitsAccordingToPosition(newPosition.x) + GridManager.instance.GetPivotMoveMargin();

            selectedNote.position = newPosition;

            float distanceBetweenNote = GridManager.instance.distanceBetweenNotes;
            float howManySnapUnitsAccordingToPosition = HowManySnapUnitsAccordingToPosition(InputManager.instance.GetTapPosition().x) + GridManager.instance.GetPivotMoveMargin();
            float distancePerUnit = distanceBetweenNote * snapValue;




            int whichNoteBordarNoteHasToSnap = (int)(howManySnapUnitsAccordingToPosition / distancePerUnit) + 1;

            selectedNote.GetComponent<NoteManager>().noteTickValue = (whichNoteBordarNoteHasToSnap-1) * (int)(32*snapValue) ;
            NoteTickValueText.text = selectedNote.GetComponent<NoteManager>().noteTickValue.ToString();
            selectedNote.GetComponent<NoteManager>().noteSpeed = whichNoteBordarNoteHasToSnap * GridManager.instance.zoomSpeed;

            // Debug.Log(whichNoteBordarNoteHasToSnap.ToString()+" Live NoteBordarCal");
        }
    }
     private void Update()
    {

        NoteMovement();
        float distancePerTick = (GridManager.instance.distanceBetweenNotes) / 32f;
        Vector2 imaginaryPositionBasedOnMargin = new Vector2(InputManager.instance.GetTapPosition().x - (GridManager.instance.GetPivotMoveMargin()), InputManager.instance.GetTapPosition().y);


        int tickNo =(int) (imaginaryPositionBasedOnMargin.x / distancePerTick);
  
          if (toBeStretchedNoteRight != null)
          {
               Vector3 currentMousePosition = InputManager.instance.GetTapPosition();
               


            
               RectTransform RightRectNote = toBeStretchedNoteRight.GetChild(0).GetComponent<RectTransform>();

              BoxCollider2D noteCollider = toBeStretchedNoteRight.transform.GetChild(0).GetComponent<BoxCollider2D>();
            noteEndAtCurrentPos = toBeStretchedNoteRight.GetComponent<NoteManager>().transform.position.x + noteCollider.bounds.size.x;

            int noteEndAtCurrentTickNo = (int)(noteEndAtCurrentPos / distancePerTick);
            int noteAtCurrentBordar = (int)(noteEndAtCurrentTickNo / (32 * snapValue));  //32 ---> 4     //
               float deltaMovement = currentMousePosition.x - lastMousePosition.x;

               float DistancePerUnit = GridManager.instance.distanceBetweenNotes * snapValue;

             float howFarCurrenMousePositionFromTheLastSnapUnit = currentMousePosition.x % (DistancePerUnit/2);



            var AbsoluteBordarNo = (int)(tickNo / (32 * snapValue));

            // 5  4  1

            var BoradarNoRelativeToNote = ((AbsoluteBordarNo+1) - noteAtCurrentBordar)+1;

            Debug.Log((AbsoluteBordarNo).ToString()+" bordar no ");


            Debug.Log(BoradarNoRelativeToNote.ToString() + "Bordar no rel");
               if (BoradarNoRelativeToNote > ifGreater )
               {

       
                 //   if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width >= 0)
                    {
                         ifGreater++;
                     
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Width += (startNoteDistance * snapValue) * 1.5f;
                         toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor += (startNoteDistance * snapValue) * 0.75f;



             
                        toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration += howManyTicksPerUnitBasedOnSnapValue;
                    
                    // Debug.Log("Note Duration :" + toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration.ToString());
                      
                    

               
                         NoteDurationValueText.text = toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration.ToString();

                        //if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width > 5)
                         {
                  
                              RightRectNote.sizeDelta = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                              RightRectNote.anchoredPosition = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor, RightRectNote.anchoredPosition.y);
                              
                              RightRectNote.GetComponent<BoxCollider2D>().size = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                       
                    }
                    }



               }

               else if (BoradarNoRelativeToNote < ifGreater)
               {
                    ifGreater--;

                    if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width >= 0)
                    {
                    
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Width -=(startNoteDistance*snapValue) * 1.5f;
                         toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor -= (startNoteDistance * snapValue) * 0.75f;
                         toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration -= howManyTicksPerUnitBasedOnSnapValue;


                    //corner case
                    if (toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration <= 0)
                    {
                        toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration =  (int)(32*snapValue);
                    }




                         NoteDurationValueText.text = toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration.ToString();
                   
                         if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width > 20)
                         {
                              RightRectNote.sizeDelta = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                              RightRectNote.anchoredPosition = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor, RightRectNote.anchoredPosition.y);
                              RightRectNote.GetComponent<BoxCollider2D>().size = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                         }
                    }



               }

               lastMousePosition.x = currentMousePosition.x;

          }
     }


     int HowManyTicksPerUnitBasedOnSnapValue(float snapValue)
     {
          int totalTicksInWholeNote = 32;
          float tickValuefloat = totalTicksInWholeNote * snapValue;
          int TickValue = Mathf.RoundToInt(tickValuefloat);
          return TickValue;
     }

     private void UiChangeOnTapEndTrigger(object sender, InputManager.TapInputEventArgs e)
     {
          i = 0;
          ifGreater = 0;
          if (selectedNote != null)
          {
               if (previousPosition < selectedNote.position.x)
               {
   
                    int howManySnapUnitsNoteMovedTowardsRight = (int)((selectedNote.position.x - previousPosition) / startNoteDistance);

               //     selectedNote.GetComponent<NoteManager>().noteTickValue += howManyTicksPerUnitBasedOnSnapValue * howManySnapUnitsNoteMovedTowardsRight;
                //    NoteTickValueText.text = selectedNote.GetComponent<NoteManager>().noteTickValue.ToString();
               }
               else if (previousPosition > selectedNote.position.x)
               {

                    int howManySnapUnitsNoteMovedTowardsLeft = (int)((previousPosition - selectedNote.position.x) / startNoteDistance);
                 //   selectedNote.GetComponent<NoteManager>().noteTickValue -= howManyTicksPerUnitBasedOnSnapValue * howManySnapUnitsNoteMovedTowardsLeft;
              //      NoteTickValueText.text = selectedNote.GetComponent<NoteManager>().noteTickValue.ToString();
               }
               SnapNoteBackToTrack(e);
               selectedNote = null;

          }

          else if (toBeStretchedNoteLeft != null || toBeStretchedNoteRight != null)
          {
               toBeStretchedNoteRight = null;
               toBeStretchedNoteLeft = null;
               isStretching = false;
          }

          else
          {
               PlaceNoteIfPossible(e);
          }
     }

     private void SnapNoteBackToTrack(InputManager.TapInputEventArgs e)
     {
          PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());

          mPointerEventData.position = e.TouchPosition;

          rayCaster.Raycast(mPointerEventData, raycastOutput);

          foreach (RaycastResult r in raycastOutput)
          {

               if (r.gameObject.TryGetComponent<TrackManager>(out TrackManager trackManager))
               {

                    selectedNote.transform.position = new Vector3(selectedNote.transform.position.x, trackManager.transform.position.y, trackManager.transform.position.z);
                    selectedNote.SetParent(trackManager.transform);
                    selectedNote.transform.GetComponent<NoteManager>().SetNoteNumber(trackManager.GetTrackNoteNumber());
               }

          }
          raycastOutput.Clear();
     }
    float HowManySnapUnitsAccordingToPosition(float position)
    {

        float distanceBetweenNote = GridManager.instance.distanceBetweenNotes;
        snapDistance = distanceBetweenNote * snapValue;

    
        return Mathf.RoundToInt((position / snapDistance)) * snapDistance;


        /*   tickValue = CalculateTickValue(snapValue);
           float tickSize = 1f / tickValue;

           Debug.Log(GridManager.instance.distanceBetweenNotes);
           tickSize = tickSize / ((GridManager.instance.distanceBetweenNotes)/32);
           float nearestTick = Mathf.Round(position * tickSize) / (tickSize);
           Debug.Log("Snap");
   */
        /*     return nearestTick;*/

        //800   /0.03125
    }

    int GetNoteBordar(InputManager.TapInputEventArgs e)
    {
        float distanceBetweenNote = GridManager.instance.distanceBetweenNotes;
        float howManySnapUnitsAccordingToPosition = HowManySnapUnitsAccordingToPosition(e.TouchPosition.x);
        float distancePerUnit = distanceBetweenNote * snapValue;




        int whichNoteBordarNoteHasToSnap = (int)(howManySnapUnitsAccordingToPosition / distancePerUnit);
        return whichNoteBordarNoteHasToSnap;
    }

    private void PlaceNoteIfPossible(InputManager.TapInputEventArgs e)
     {
         
          PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());
          mPointerEventData.position = e.TouchPosition;
          rayCaster.Raycast(mPointerEventData, raycastOutput);

          foreach (RaycastResult r in raycastOutput)
          {
               //Making Note
               if (r.gameObject.TryGetComponent<NoteManager>(out NoteManager noteManager))
               {
                    isNoteOnTap = true;
               }

               if (r.gameObject.TryGetComponent<TrackManager>(out TrackManager trackManager))
               {
                    if (!isNoteOnTap)
                    {
                         Vector2 snappedPosition = new Vector2(HowManySnapUnitsAccordingToPosition(e.TouchPosition.x) + GridManager.instance.GetPivotMoveMargin(), e.TouchPosition.y);




                    int whichNoteBordarNoteHasToSnap = GetNoteBordar(e);
                   
                    int initialTickValue = (int)(snappedPosition.x / snapDistance*howManyTicksPerUnitBasedOnSnapValue);
                         NoteTickValueText.text = initialTickValue.ToString();


                    //corner case
                    NoteDurationValueText.text = (32*snapValue).ToString();

                    OnPlaceNoteTrigger?.Invoke(this, new OnTrackTriggerEventArgs
                    {
                        TrackSelected = trackManager,
                        NotePlacePosition = snappedPosition,
                        InitialNoteTickValue = initialTickValue,
                        noteBordar = whichNoteBordarNoteHasToSnap + 1
                    }) ;

                    }
                    else
                    {
                         isNoteOnTap = false;
                    }
               }

          }


          raycastOutput.Clear();

     }

     private void OnDisable()
     {
          InputManager.OnTapEndInput -= UiChangeOnTapEndTrigger;
          InputManager.OnTapStartInput -= UiChangeOnTapStartTrigger;
     }
}