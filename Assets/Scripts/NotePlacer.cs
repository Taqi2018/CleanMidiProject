using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlacer : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform notePrefab;

    void Start()
    {
        UiManager.OnPlaceNoteTrigger += NotePlaceOnSelectedTrack;
    }

    private void NotePlaceOnSelectedTrack(object sender, UiManager.OnTrackTriggerEventArgs e)
    {
        Vector3 notePlacePosition = new Vector3(e.NotePlacePosition.x, e.TrackSelected.transform.position.y, e.TrackSelected.transform.position.z);
        Transform note=Instantiate(notePrefab, notePlacePosition, Quaternion.identity);
        note.SetParent(e.TrackSelected.transform);
      
        note.transform.localScale = Vector3.one;
        note.transform.GetComponent<NoteManager>().SetNoteNumber(e.TrackSelected.GetTrackNoteNumber());
/*        if(note.TryGetComponent<RectTransform>(out RectTransform r)){
            // Get the current sizeDelta of the RectTransform
            Vector2 sizeDelta = r.sizeDelta;

        *//*    // Modify the height component of sizeDelta
            sizeDelta.y = 1;*//*

            // Assign the modified sizeDelta back to the RectTransform
            r.sizeDelta = sizeDelta;

        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        UiManager.OnPlaceNoteTrigger -= NotePlaceOnSelectedTrack;
    }
}
