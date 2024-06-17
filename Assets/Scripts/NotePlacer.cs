using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePlacer : MonoBehaviour
{
     // Start is called before the first frame update
     public Transform notePrefab;
     public float noteXScale;

     void Start()
     {
          UiManager.OnPlaceNoteTrigger += NotePlaceOnSelectedTrack;
     }

     private void NotePlaceOnSelectedTrack(object sender, UiManager.OnTrackTriggerEventArgs e)
     {


          Vector3 notePlacePosition = new Vector3(e.NotePlacePosition.x, e.TrackSelected.transform.position.y, e.TrackSelected.transform.position.z);
          Transform note = Instantiate(notePrefab, notePlacePosition, Quaternion.identity);

          note.SetParent(e.TrackSelected.transform);

         noteXScale = ((GridManager.instance.distanceBetweenNotes / 2) * 0.67f) / 150f;
        note.transform.localScale = new Vector3(noteXScale, 1, 1);
          note.transform.GetComponent<NoteManager>().SetNoteNumber(e.TrackSelected.GetTrackNoteNumber());
          note.transform.GetComponent<NoteManager>().noteTickValue = e.InitialNoteTickValue;
        note.transform.GetComponent<NoteManager>().noteSpeed = e.noteBordar * GridManager.instance.zoomSpeed;
          /*UiManager.NoteDurationValueText.text = note.transform.GetComponent<NoteManager>().noteDuration.ToString();*/

     }

    private void OnDisable()
     {
          UiManager.OnPlaceNoteTrigger -= NotePlaceOnSelectedTrack;
     }
}
