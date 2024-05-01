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
     private float rightSide;
     private bool isStretching;
     public float clickMargin = 1f;
     private Vector3 lastMousePosition;
     public float snapValue;
     [SerializeField] int tickValue;
     float objectWidth;
     public float snapDistance;
     int ifGreater;
     private float newWidth;
     private float newAnchor;
     private int i;
     public Text NoteTickValueText;
     public Text NoteDurationValueText;

     public class OnTrackTriggerEventArgs : EventArgs
     {
          public TrackManager TrackSelected;
          public Vector2 NotePlacePosition;
          public int InitialNoteTickValue;
     }

     private void OnEnable()
     {
          InputManager.OnTapEndInput += UiChangeOnTapEndTrigger;
          InputManager.OnTapStartInput += UiChangeOnTapStartTrigger;
     }

     void Start()
     {
          rayCaster = canvas.GetComponent<GraphicRaycaster>();
          raycastOutput = new List<RaycastResult>();
          isStretching = false;
     }

     private void UiChangeOnTapStartTrigger(object sender, InputManager.TapInputEventArgs e)
     {
          tickValue = CalculateTickValue(snapValue);
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
                    NoteDurationValueText.text = noteManager.noteDuration.ToString();
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
                    Debug.Log("This is note");
                    BoxCollider2D noteCollider = noteManager.transform.GetChild(0).GetComponent<BoxCollider2D>();

                    //leftSide = noteManager.transform.position.x;
                    rightSide = noteManager.transform.position.x + noteCollider.bounds.size.x + clickMargin;
                    /*if (e.TouchPosition.x <= leftSide)
                    {
                         toBeStretchedNoteLeft = noteManager.transform;
                         Debug.Log("HEYYYYYYYYYYYYY IM LEFT");
                         selectedNote = null;
                         isStretching = true;
                         lastMousePosition.x = e.TouchPosition.x;
                    }*/
                    if (e.TouchPosition.x >= rightSide)
                    {
                         toBeStretchedNoteRight = noteManager.transform;
                         Debug.Log("HEYYYYYYYYYYYYY IM Right");
                         selectedNote = null;
                         isStretching = true;
                         lastMousePosition.x = e.TouchPosition.x;
                    }

               }
          }
          raycastOutput.Clear();
     }

     private void Update()
     {
          if (selectedNote != null)
          {
               Vector3 newPosition = InputManager.instance.GetTapPosition();

               Snap(newPosition.x);
               newPosition.x = Snap(newPosition.x) + GridManager.instance.GetPivotMoveMargin();
               
               selectedNote.position = newPosition;
          }

          /*if (toBeStretchedNoteLeft != null)
          {
               Vector3 currentMousePosition = InputManager.instance.GetTapPosition();
               RectTransform LeftRectNote = toBeStretchedNoteLeft.GetComponent<RectTransform>();
               float dragDistance = currentMousePosition.x - lastMousePosition.x;
               float newWidth = LeftRectNote.sizeDelta.x - dragDistance * 1;
               float newAnchor = LeftRectNote.anchoredPosition.x + dragDistance * 0.5f;

               if (newWidth > 40)
               {
                    LeftRectNote.sizeDelta = new Vector2(newWidth, LeftRectNote.sizeDelta.y);
                    LeftRectNote.anchoredPosition = new Vector2(newAnchor, LeftRectNote.anchoredPosition.y);
                    LeftRectNote.GetComponent<BoxCollider2D>().size = new Vector2(newWidth, LeftRectNote.sizeDelta.y);
               }
               lastMousePosition = currentMousePosition;
          }*/

          if (toBeStretchedNoteRight != null)
          {
               Vector3 currentMousePosition = InputManager.instance.GetTapPosition();

               RectTransform RightRectNote = toBeStretchedNoteRight.GetChild(0).GetComponent<RectTransform>();

               float deltaMovement = currentMousePosition.x - lastMousePosition.x;

               if (Mathf.Round(currentMousePosition.x) % (GridManager.instance.distanceBetweenNotes*snapValue) == 0f & deltaMovement > 0)
               {
                    i++;
               }

               if (Mathf.Round(currentMousePosition.x) % (GridManager.instance.distanceBetweenNotes * snapValue) == 0f & deltaMovement < 0)
               {
                    i--;
               }
               /*Debug.Log("Delta Movement: " + deltaMovement);*/
               /*Debug.Log("Hi" + ((GridManager.instance.distanceBetweenNotes * snapValue)-1));
               Debug.Log((int)(Mathf.Round(currentMousePosition.x % GridManager.instance.distanceBetweenNotes)));

               Debug.Log((((int)(currentMousePosition.x / snapDistance))).ToString());*/
               /*Debug.Log(Mathf.Round(currentMousePosition.x) % (GridManager.instance.distanceBetweenNotes * snapValue));*/
               if (i > ifGreater)
               {


                    Debug.Log("MoveNote to " + ifGreater.ToString() + " position");
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Width += snapDistance * 3f;
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor += snapDistance * 1.5f;
                    toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration += tickValue;
                    NoteDurationValueText.text = toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration.ToString();
                    Debug.Log("HIIIIIIII" + snapDistance);
                    if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width > 40)
                    {
                         RightRectNote.sizeDelta = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                         RightRectNote.anchoredPosition = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor, RightRectNote.anchoredPosition.y);
                         RightRectNote.GetComponent<BoxCollider2D>().size = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                    }
                   
                    ifGreater++;
               }

               else if (i < ifGreater)
               {
                    
                    Debug.Log("MoveNote to " + ifGreater.ToString() + " position");
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Width -= snapDistance * 3f;
                    toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor -= snapDistance * 1.5f;
                    toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration -= tickValue;
                    NoteDurationValueText.text = toBeStretchedNoteRight.GetComponent<NoteManager>().noteDuration.ToString();
                    Debug.Log("HIIIIIIII----------" + snapDistance);
                    if (toBeStretchedNoteRight.GetComponent<NoteManager>().Width > 40)
                    {
                         RightRectNote.sizeDelta = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                         RightRectNote.anchoredPosition = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Anchor, RightRectNote.anchoredPosition.y);
                         RightRectNote.GetComponent<BoxCollider2D>().size = new Vector2(toBeStretchedNoteRight.GetComponent<NoteManager>().Width, RightRectNote.sizeDelta.y);
                    }

                    ifGreater--;

               }

               lastMousePosition.x = currentMousePosition.x;

          }
     }

     float Snap(float position)
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
     }

     int CalculateTickValue(float snapValue)
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
                    selectedNote.GetComponent<NoteManager>().noteTickValue += 8;
                    NoteTickValueText.text = selectedNote.GetComponent<NoteManager>().noteTickValue.ToString();
               }
               else if (previousPosition > selectedNote.position.x)
               {
                    selectedNote.GetComponent<NoteManager>().noteTickValue += -8;
                    NoteTickValueText.text = selectedNote.GetComponent<NoteManager>().noteTickValue.ToString();
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
                         Vector2 snappedPosition = new Vector2(Snap(e.TouchPosition.x), e.TouchPosition.y);
                         int initialTickValue = (int)(snappedPosition.x / snapDistance*8);
                         NoteTickValueText.text = initialTickValue.ToString();
                     
                         OnPlaceNoteTrigger?.Invoke(this, new OnTrackTriggerEventArgs
                         {
                              TrackSelected = trackManager,
                              NotePlacePosition = snappedPosition,
                              InitialNoteTickValue = initialTickValue
                         });

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