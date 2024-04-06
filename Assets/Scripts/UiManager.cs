/*using System;
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
    public static EventHandler <OnTrackTriggerEventArgs>OnPlaceNoteTrigger;
    private bool isNoteOnTap;
    private bool isTapStarted;
    private Transform selectedNote;
    private bool isTrackParent;
    

    public class OnTrackTriggerEventArgs : EventArgs
    {
        public TrackManager TrackSelected;
        public Vector2 NotePlacePosition;

    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        InputManager.OnTapEndInput += UiChangeOnTapEndTrigger;
        InputManager.OnTapStartInput += UiChangeOnTapStartTrigger;

 
       
    }

    private void UiChangeOnTapStartTrigger(object sender, InputManager.TapInputEventArgs e)
    {
        MoveNoteIfPossible(e);
    }

    private void MoveNoteIfPossible(InputManager.TapInputEventArgs e)
    {
        PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());

        mPointerEventData.position = e.TouchPosition;



        *//*
                rayCaster.Raycast()*//*

        rayCaster.Raycast(mPointerEventData, raycastOutput);

        foreach (RaycastResult r in raycastOutput)
        {



            //Making Note
            if (r.gameObject.TryGetComponent<NoteManager>(out NoteManager noteManager))
            {

                noteManager.transform.SetParent(canvas.transform);

                selectedNote = noteManager.transform;

            }

           

        }


        raycastOutput.Clear();
    }

    void Start()
    {
    
        rayCaster = canvas.GetComponent<GraphicRaycaster>();
        raycastOutput = new List<RaycastResult>();
        isTrackParent = true;
    }



    private void UiChangeOnTapEndTrigger(object sender, InputManager.TapInputEventArgs e)
    {
        Debug.Log(e.TouchPosition);

        if (selectedNote != null)
        {
            SnapNoteBackToTrack(e);

            selectedNote = null;
        }


        PlaceNoteIfPossible(e);



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
                selectedNote.transform.position = new Vector3(e.TouchPosition.x, trackManager.transform.position.y, trackManager.transform.position.z);
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



        *//*
                rayCaster.Raycast()*//*

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


                        Debug.Log("PLACE");

                        OnPlaceNoteTrigger?.Invoke(this, new OnTrackTriggerEventArgs
                        {
                            TrackSelected = trackManager,
                            NotePlacePosition = e.TouchPosition
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


    private void Update()
    {
        if (selectedNote != null)
        {
            selectedNote.position = InputManager.instance.input.MidiInput.TapPos.ReadValue<Vector2>();
        }
    }
    private void OnDisable()
    {
        InputManager.OnTapEndInput -= UiChangeOnTapEndTrigger;
        InputManager.OnTapStartInput -= UiChangeOnTapStartTrigger;
    }
}
*/


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
    private bool isTrackParent;
    private Transform toBeStretchedNoteLeft, toBeStretchedNoteRight;
    private float leftSide;
    private float rightSide;
    private bool isStretching;
    public float clickMargin = 1f;
    private Vector3 lastMousePosition;
    public float snapValue = 32;
    [SerializeField] int tickValue;
    float objectWidth;
    public class OnTrackTriggerEventArgs : EventArgs
    {
        public TrackManager TrackSelected;
        public Vector2 NotePlacePosition;

    }




    // Start is called before the first frame update

    private void OnEnable()
    {
        InputManager.OnTapEndInput += UiChangeOnTapEndTrigger;
        InputManager.OnTapStartInput += UiChangeOnTapStartTrigger;
    }

    void Start()
    {

        rayCaster = canvas.GetComponent<GraphicRaycaster>();
        raycastOutput = new List<RaycastResult>();
        isTrackParent = true;
        isStretching = false;

    }

    private void UiChangeOnTapStartTrigger(object sender, InputManager.TapInputEventArgs e)
    {
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

            if (r.gameObject.TryGetComponent<NoteManager>(out NoteManager noteManager))
            {

                noteManager.transform.SetParent(canvas.transform);

                selectedNote = noteManager.transform;


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
            if (r.gameObject.TryGetComponent<NoteManager>(out NoteManager noteManager))
            {

                BoxCollider2D noteCollider = noteManager.GetComponent<BoxCollider2D>();

                leftSide = noteManager.transform.position.x - (noteCollider.bounds.extents.x + clickMargin);
                rightSide = noteManager.transform.position.x + (noteCollider.bounds.extents.x + clickMargin);

                if (e.TouchPosition.x <= leftSide)
                {
                    toBeStretchedNoteLeft = noteManager.transform;
                    Debug.Log("HEYYYYYYYYYYYYY IM LEFT");
                    selectedNote = null;
                    isStretching = true;
                    lastMousePosition.x = e.TouchPosition.x;
                }
                else if (e.TouchPosition.x >= rightSide)
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


            newPosition.x = SnapToGrid(newPosition.x);


            selectedNote.position = newPosition;
        }

        if (toBeStretchedNoteLeft != null)
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

        }

        if (toBeStretchedNoteRight != null)
        {
            Vector3 currentMousePosition = InputManager.instance.GetTapPosition();


            RectTransform RightRectNote = toBeStretchedNoteRight.GetComponent<RectTransform>();


            float dragDistance = currentMousePosition.x - lastMousePosition.x;

            float newWidth = RightRectNote.sizeDelta.x + dragDistance * 1;
            float newAnchor = RightRectNote.anchoredPosition.x + dragDistance * 0.5f;

            if (newWidth > 40)
            {
                RightRectNote.sizeDelta = new Vector2(newWidth, RightRectNote.sizeDelta.y);
                RightRectNote.anchoredPosition = new Vector2(newAnchor, RightRectNote.anchoredPosition.y);
                RightRectNote.GetComponent<BoxCollider2D>().size = new Vector2(newWidth, RightRectNote.sizeDelta.y);
            }

            lastMousePosition = currentMousePosition;
        }
    }

    float SnapToGrid(float position)
    {

        tickValue = CalculateTickValue(snapValue);
        float tickSize = 1f / tickValue;
        tickSize = tickSize / 4;
        float nearestTick = Mathf.Round(position * tickSize) / (tickSize);
        Debug.Log("Snap");

        return nearestTick;
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
        Debug.Log(e.TouchPosition);

        if (selectedNote != null)
        {
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
                    Vector2 snappedPosition = new Vector2(SnapToGrid(e.TouchPosition.x), e.TouchPosition.y);
                    Debug.Log("PLACE");

                    OnPlaceNoteTrigger?.Invoke(this, new OnTrackTriggerEventArgs
                    {
                        TrackSelected = trackManager,
                        NotePlacePosition = snappedPosition
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