using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Level1Music))]
public class KeyQueue {
    Queue lastKeys;
    int currentSize;

    public KeyQueue() {
        lastKeys = new Queue(3);
        currentSize = 0;
    }

    public void newNotePlayed(int newNote) {
        if (currentSize > 2)
        {
            lastKeys.Dequeue();
            currentSize--;
        }      
        lastKeys.Enqueue(newNote);
        currentSize++;
        Debug.Log(currentSize);
    }

    public void clearQueue() {
        lastKeys.Clear();
    }

    public int[] getNotes() {

        int[] notesArray = new int[3];
        for (int i = 0; i < currentSize; i++) {
            notesArray[i] = (int)lastKeys.Dequeue();
        }

        currentSize = 0;
        return notesArray;
    }
}

public class PlayerAbilitiesController : MonoBehaviour {

    KeyQueue keyQueue;
    AudioSource[] notes;
    Level1Music music;
    Level1Music.Key songKey;
    int startingNote;
    int[] lastThreeNotes;

    // Use this for initialization
    void Start () {
        keyQueue = new KeyQueue();
        notes = GameObject.FindGameObjectWithTag("Keytar").GetComponents<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<Level1Music>();
        songKey = music.getSongKey();
        startingNote = music.getNoteKey();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            notes[startingNote].Play();
            keyQueue.newNotePlayed(startingNote);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            notes[startingNote+2].Play();
            keyQueue.newNotePlayed(startingNote+2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //For Major keys play 4th note
            if (songKey == Level1Music.Key.major)
            {
                notes[startingNote+4].Play();
                keyQueue.newNotePlayed(startingNote + 4);
            }
            //For Minor keys play 3th note
            if (songKey == Level1Music.Key.minor)
            {
                notes[startingNote+3].Play();
                keyQueue.newNotePlayed(startingNote + 3);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            notes[startingNote+5].Play();
            keyQueue.newNotePlayed(startingNote + 5);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            notes[startingNote+7].Play();
            keyQueue.newNotePlayed(startingNote + 7);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //For Major keys play 9thth note
            if (songKey == Level1Music.Key.major)
            {
                notes[startingNote + 9].Play();
                keyQueue.newNotePlayed(startingNote + 9);
            }
            //For Minor keys play 8th note
            if (songKey == Level1Music.Key.minor)
            {
                notes[startingNote + 8].Play();
                keyQueue.newNotePlayed(startingNote + 8);
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //For Major keys play 11th note
            if (songKey == Level1Music.Key.major)
            {
                notes[startingNote + 11].Play();
                keyQueue.newNotePlayed(startingNote + 11);
            }
            //For Minor keys play 10th note
            if (songKey == Level1Music.Key.minor)
            {
                notes[startingNote + 10].Play();
                keyQueue.newNotePlayed(startingNote + 10);
            }
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            notes[startingNote + 12].Play();
            keyQueue.newNotePlayed(startingNote + 12);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            lastThreeNotes = keyQueue.getNotes();
        }
    }  
    
}
