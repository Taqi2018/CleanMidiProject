using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;

public class ScriptableMusic : MonoBehaviour
{


// MPTK component able to play a stream of midi events
// Need to be public to be visible in the inspector
public MidiStreamPlayer midiStreamPlayer;

    public MPTKEvent NotePlaying { get; private set; }

    private void Start()
    {



        midiStreamPlayer = FindObjectOfType<MidiStreamPlayer>();
        if (midiStreamPlayer == null)
            Debug.LogWarning("Can't find a MidiStreamPlayer Prefab in the current Scene Hierarchy. Add it with the MPTK menu.");

        if (midiStreamPlayer != null)
        {
            /*if (!midiStreamPlayer.OnEventSynthStarted.HasEvent())*/
                // The method EndLoadingSynth will be called when the synth is ready
                midiStreamPlayer.OnEventSynthStarted.AddListener(EndLoadingSynth);
        }
        else
            Debug.LogWarning("No Stream Midi Player associed to this game object");
    }

  

    public void EndLoadingSynth(string name)
{
        // Change the default instrument when synth is started.

           NotePlaying =new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn, // midi command
                Value = 48, // from 0 to 127, 48 for C4, 60 for C5, ...
                Channel = 0, // from 0 to 15, 9 reserved for drum
                Duration = 10, // note duration in millisecond, -1 to play undefinitely, MPTK_StopChord to stop
                Velocity = 100, // from 0 to 127, sound can vary depending on the velocity
                Delay = 0, // delay in millisecond before playing the note
            } ;
        midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

     
    }

    void Update()
    {

        /*   NotePlaying = new MPTKEvent()
           {
               Command = MPTKCommand.NoteOn,
               Value = 60,
               Channel = 0,
               Duration = 1, // 9999 seconds but stop by the new note. See before.
               Velocity = 100 // Sound can vary depending on the velocity
           };
           midiStreamPlayer.MPTK_PlayEvent(NotePlaying);


           if (NotePlaying != null)
           {
               // Stop the note (method to simulate a real human on a keyboard : duration is not known when note is triggered)
               midiStreamPlayer.MPTK_StopEvent(NotePlaying);
               NotePlaying = null;
           }*/
    }








}

