using UnityEngine;
using MidiPlayerTK;
using MPTK.NAudio.Midi;
using System.Collections.Generic;
using System;

using System.IO;
public class TestingNoteWriter : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    private MidiFile midiFile;
    public static TestingNoteWriter i;
    public MPTKWriter midiWriter;
    private List<MPTKEvent> midiEvents;
    public MidiFilePlayer midiFilePlayer;
    private DateTime startPlaying;

    private void Start()
    {
        i = this;
        // Create a new instance of MPTKWriter
        midiWriter = new MPTKWriter();
    }

    public void AddNotes()
    {
 
       
        int track1 = 1;

        int channel0 = 0; // we are using only one channel in this demo

        long absoluteTime = 0;

        midiWriter.AddTimeSignature(0, 0, 4, 2);

        // 240 is the default. A classical value for a Midi. define the time precision.
        int ticksPerQuarterNote = midiWriter.DeltaTicksPerQuarterNote;

        midiWriter.AddNote(track1, absoluteTime, channel0, 60, 50, ticksPerQuarterNote);
/*
        // Next note will be played one quarter after the previous
        absoluteTime += ticksPerQuarterNote;
        midiWriter.AddNote(track1, absoluteTime, channel0, 60, 50, ticksPerQuarterNote);

        absoluteTime += ticksPerQuarterNote;
        midiWriter.AddNote(track1, absoluteTime, channel0, 60, 50, ticksPerQuarterNote);

        absoluteTime += ticksPerQuarterNote;
        midiWriter.AddNote(track1, absoluteTime, channel0, 60, 50, ticksPerQuarterNote);*/


        WriteMidiSequenceToFile();
    }




    private void WriteMidiSequenceToFile()
    {
        // build the path + filename to the midi
        string filename = Path.Combine(Application.persistentDataPath, "My1" + ".mid");
        Debug.Log("Write Midi file:" + filename);

        // Write the midi file
        midiWriter.WriteToFile(filename);
    }

    public void PlayDirectlyMidiSequence()
    {
        // Play midi with the MidiExternalPlay prefab without saving midi in a file
        MidiFilePlayer midiPlayer = FindObjectOfType<MidiFilePlayer>();

        midiPlayer.MPTK_Stop();
        midiWriter.MidiName = "My";

        midiPlayer.OnEventStartPlayMidi.RemoveAllListeners();
        midiPlayer.OnEventStartPlayMidi.AddListener((string midiname) =>
        {
            startPlaying = DateTime.Now;
            Debug.Log($"Start playing {midiname} at {startPlaying}");
        });

        midiPlayer.OnEventEndPlayMidi.RemoveAllListeners();
        midiPlayer.OnEventEndPlayMidi.AddListener((string midiname, EventEndMidiEnum reason) =>
        {
            Debug.Log($"End playing {midiname} {reason} Duration={(DateTime.Now - startPlaying).TotalSeconds:F3}");
        });

        midiPlayer.OnEventNotesMidi.RemoveAllListeners();
        midiPlayer.OnEventNotesMidi.AddListener((List<MPTKEvent> events) =>
        {
            foreach (MPTKEvent midievent in events)
                Debug.Log($"At {midievent.RealTime:F1} ms play: {midievent.ToString()}");
        });


        midiPlayer.MPTK_Loop = true;

        /*    // Sort the events by ascending absolute time (optional)
            mfw.MPTK_SortEvents();
            mfw.MPTK_Debug();*/

        // Send the midi sequence to internal midi sequencer

        midiFilePlayer.MPTK_MidiName = "My1.mid";

       midiWriter.LoadFromFile(Path.Combine(Application.persistentDataPath, "My1.mid" ));
  

     
        midiPlayer.MPTK_Play(midiWriter);
      /*  midiPlayer.MPTK_Play(midiWriter);*/
    }


    public void PlayNotes()
    {
        // Get all the MIDI events from the writer
        midiEvents = midiWriter.MPTK_MidiEvents;



        /*    midiFilePlayer.MPTK_Play();*/
        // Play the MIDI events
        midiStreamPlayer.MPTK_PlayEvent(midiEvents);



    }

 
}