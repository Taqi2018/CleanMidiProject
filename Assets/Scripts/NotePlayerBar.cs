using System;
using System.Collections;
using System.Collections.Generic;
using MidiPlayerTK;
using UnityEngine;
using UnityEngine.UI;

public class NotePlayerBar : MonoBehaviour
{
    public MidiFilePlayer midiFilePlayer;
    public MidiStreamPlayer midiStreamPlayer;
    [SerializeField]private float playerVelocity;
    private Vector3 initialPositionOfNotePlayer;
    private bool shouldNotePlayerMove;
    [SerializeField] int velocity;
    private MPTKEvent midiEvent;
    [SerializeField] int duration;

    public MPTKEvent NotePlaying { get; private set; }

    private void Start()
    {

        /*        PlayOneNote();*/

        NoteManager.OnNoteTrigger += PlayNote;

        initialPositionOfNotePlayer = transform.position;
        duration = 1000;
        velocity = 100;
/*
        midiEvent = TestingNoteWriter.i.midiWriter.MPTK_LastEvent;
        midiStreamPlayer.MPTK_PlayEvent(midiEvent);*/
    }

    private void PlayNote(object sender, NoteManager.OnNoteTriggerEventArgs e)
    {
        // Play a note C5 for 1 second on the channel 0
        NotePlaying = new MPTKEvent()
        {
            Command = MPTKCommand.NoteOn,
            Value = e.noteNumber,  //C5
            Channel = 0,
            Duration =(int)(duration),
            Velocity = ((int)velocity),
            Delay = 0,
        };
        midiStreamPlayer.MPTK_PlayEvent(NotePlaying);
    }

    private void PlayOneNote(int noteNumber)
    {

    }
    private void Update()
    {
        if (shouldNotePlayerMove)
        {
            transform.Translate(new Vector3(playerVelocity, 0, 0));
        }
    }

    public void Reset()
    {
        transform.position = initialPositionOfNotePlayer;
        shouldNotePlayerMove=false;
    }
    public void Play()
    {
     
        shouldNotePlayerMove = true;
    }
    public void Pause()
    {

        shouldNotePlayerMove = false;
    }
    public void SetDuration(string duration_)
    {
        duration=int.Parse(duration_);
    }

    public void SetVelcoity(string velocity_)
    {
        velocity= int.Parse(velocity_);
    }
    public void OnDisable()
    {
        NoteManager.OnNoteTrigger -= PlayNote;
    }











}

