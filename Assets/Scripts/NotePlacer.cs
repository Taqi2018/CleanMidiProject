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
          Transform note = Instantiate(notePrefab, notePlacePosition, Quaternion.identity);
          note.SetParent(e.TrackSelected.transform);

          note.transform.localScale = Vector3.one;
          note.transform.GetComponent<NoteManager>().SetNoteNumber(e.TrackSelected.GetTrackNoteNumber());
     }


     private void OnDisable()
     {
          UiManager.OnPlaceNoteTrigger -= NotePlaceOnSelectedTrack;
     }
}
