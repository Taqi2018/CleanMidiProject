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



        /*
                rayCaster.Raycast()*/

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

        selectedNote = null;
        PlaceNoteIfPossible(e);
       


    }

    private void PlaceNoteIfPossible(InputManager.TapInputEventArgs e)
    {
        PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());

        mPointerEventData.position = e.TouchPosition;



        /*
                rayCaster.Raycast()*/

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
