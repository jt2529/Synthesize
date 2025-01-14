using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimer : MonoBehaviour
{
    public AudioSource metronomeSound;
    public AudioSource song;
    public bool playMetronomeSound;
    public int beatsPerQuarterNote, totalBeatsInPattern;
    public int currentMeasure;
    public float bpm;
    public int currentBeat;
    public bool beatOn;
    private float lastSongTime, beatInterval, beatTimer, timeOfLastQuarterNote, timeOfNextQuarterNote, timeOfLastBeat, timeOfNextBeat;

    // Start is called before the first frame update
    void Start()
    {
        currentMeasure = 1;
        currentBeat = 1;
        lastSongTime = 0;
        beatOn = false;
        song.PlayDelayed(2);
    }

    // Update is called once per frame
    void Update()
    {
        BeatDetection();
    }

    public void BeatDetection()
    {
        if (song.time < lastSongTime) //This means song has looped, so reset all current variables
        {
            currentMeasure = 1;
            currentBeat = 1;
            lastSongTime = 0;
        }

        beatOn = false;

        beatInterval = 60 / bpm / beatsPerQuarterNote; //60 bpm would equal 1 beat per second, then divide by 8 to get 4 16th notes per beat
        beatTimer += song.time - lastSongTime;
        int numberOfBeatsPassed = (int)Mathf.Floor(beatTimer / beatInterval);
        lastSongTime = song.time;
        if (beatTimer >= beatInterval)
        {
            if (currentBeat >= (totalBeatsInPattern))
            {
                currentBeat = 1;
                currentMeasure++;
            }
            else
            {
                while (numberOfBeatsPassed > 0)
                {
                    if (currentBeat >= (totalBeatsInPattern))
                    {
                        currentBeat = 1;
                    }
                    currentBeat++;
                    numberOfBeatsPassed--;
                }

            }

            beatOn = true;
            beatTimer -= beatInterval;
            timeOfLastBeat = song.time - beatTimer;
            timeOfNextBeat = timeOfLastBeat + beatInterval;
            if (currentBeat % beatsPerQuarterNote == 0)
            {
                timeOfLastQuarterNote = song.time - beatTimer;
                if (timeOfLastQuarterNote < 0)
                {
                    timeOfLastQuarterNote = 0;
                }
                timeOfNextQuarterNote = timeOfLastQuarterNote + beatInterval;
                if (playMetronomeSound)
                {
                    metronomeSound.PlayDelayed(timeOfNextQuarterNote - song.time);//((beatInterval * (beatsPerQuarterNote - 1)) + (beatInterval - beatTimer)); // Hit metronome sound on the next quarter note, so that we can play the sound accurately on the downbeat
                }

            }

        }
    }




    public float GetLastQuarterNote() { return timeOfLastQuarterNote; }

    public float GetNextQuarterNote() { return timeOfNextQuarterNote; }

    public float GetLastBeat() { return timeOfLastBeat; }

    public float GetNextBeat() { return timeOfNextBeat; }
}
