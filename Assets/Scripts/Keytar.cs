using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Key { major, minor };

[RequireComponent(typeof(Music))]
public class Keytar : MonoBehaviour {

    public int currentNoteKey;
    public Key currentSongKey;
    public AudioSource[] notes;
    Queue<int> lastKeys;
    public int currentQueueSize;
    float pitchScalar;

    // Use this for initialization
    void Start () {
        notes = GetNotes();
        lastKeys = new Queue<int>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioSource[] GetNotes() {
        return this.GetComponents<AudioSource>();
    }

    public void Play(int note) {
        notes[note].loop = true;
        notes[note].Play();

        if (currentQueueSize > 2)
        {
            lastKeys.Dequeue();
            currentQueueSize--;
        }

        lastKeys.Enqueue(note);
        currentQueueSize++;
        Debug.Log(currentQueueSize);
    }

    public void Release(int note) {
        notes[note].loop = false;
    }

    public void ChangeKey(int newNoteKey, Key newSongKey) {
        if (currentNoteKey == newNoteKey && currentSongKey == newSongKey) { return; }

        int j = 0;
        for (int i = 0; i < notes.Length; i++){
            if ((j == 1 || j == 6) 
             || (j == 3 && newSongKey == Key.major) 
             || (j == 4 && newSongKey == Key.minor) 
             || (j == 8 && newSongKey == Key.major) 
             || (j == 9 && newSongKey == Key.minor) 
             || (j == 10 && newSongKey == Key.major) 
             || (j == 11 && newSongKey == Key.minor)) {
                j++;
            }
            pitchScalar = Mathf.Pow(1.05946f, newNoteKey + j);
            notes[i].pitch = pitchScalar;
            j++;
        }
        currentNoteKey = newNoteKey;
        currentSongKey = newSongKey;
    }

    public void ClearQueue()
    {
        lastKeys.Clear();
    }

    public int[] GetLastPlayed()
    {

        int[] notesArray = new int[3];
        for (int i = 0; i < currentQueueSize; i++)
        {
            notesArray[i] = (int)lastKeys.Dequeue();
        }

        currentQueueSize = 0;
        return notesArray;
    }
}
