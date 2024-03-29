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
    public static EventHandler <OnTrackTriggerEventArgs>OnTrackTrigger;
    public class OnTrackTriggerEventArgs : EventArgs
    {
        public TrackManager TrackSelected;
        public Vector2 NotePlacePosition;

    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        InputManager.OnTapInput += GetUiElementOnTapTrigger;

 
       
    }




    void Start()
    {
    
        rayCaster = canvas.GetComponent<GraphicRaycaster>();
        raycastOutput = new List<RaycastResult>();

    }



    private void GetUiElementOnTapTrigger(object sender, InputManager.TapInputEventArgs e)
    {
        Debug.Log(e.TouchPosition);

        PointerEventData mPointerEventData = new PointerEventData(GetComponent<EventSystem>());

        mPointerEventData.position = e.TouchPosition;
       
        

/*
        rayCaster.Raycast()*/

        rayCaster.Raycast(mPointerEventData, raycastOutput);

        foreach (RaycastResult r in raycastOutput)
        {
            Debug.Log(r.gameObject.name);
            if(r.gameObject.TryGetComponent<TrackManager> (out TrackManager trackManager))
            {
                Debug.Log(trackManager.GetTrackNoteNumber());

                OnTrackTrigger?.Invoke(this, new OnTrackTriggerEventArgs
                {
                    TrackSelected = trackManager,
                    NotePlacePosition = e.TouchPosition
                }) ;
                
            }
        }
        raycastOutput.Clear();



    }


    private void OnDisable()
    {
        InputManager.OnTapInput -= GetUiElementOnTapTrigger;
    }
}
