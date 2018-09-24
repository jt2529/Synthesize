using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Music))]
public class KeyQueue {
    Queue lastKeys;
    int currentSize;

    public KeyQueue() {
        lastKeys = new Queue(3);
        currentSize = 0;
    }

    public void NewNotePlayed(int newNote) {
        if (currentSize > 2)
        {
            lastKeys.Dequeue();
            currentSize--;
        }      
        lastKeys.Enqueue(newNote);
        currentSize++;
        Debug.Log(currentSize);
    }

    public void ClearQueue() {
        lastKeys.Clear();
    }

    public int[] GetNotes() {

        int[] notesArray = new int[3];
        for (int i = 0; i < currentSize; i++) {
            notesArray[i] = (int)lastKeys.Dequeue();
        }

        currentSize = 0;
        return notesArray;
    }
}

public class PlayerAbilitiesController : MonoBehaviour {

    PlayerGun gun;
    PlayerStats stats;
    KeyQueue keyQueue;
    AudioSource[] notes;
    Music music;
    Music.Key songKey;
    int startingNote;
    int[] lastThreeNotes;
    float pitchScalar;

    // Use this for initialization
    void Start () {
        gun = GetComponent<PlayerGun>();
        keyQueue = new KeyQueue();
        notes = GameObject.FindGameObjectWithTag("Keytar").GetComponents<AudioSource>();
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<Music>();
        songKey = music.getSongKey();
        startingNote = music.getNoteKey();
        stats = GetComponent<PlayerStats>();

        pitchScalar = Mathf.Pow(1.05946f, startingNote);
        notes[0].pitch = pitchScalar;

        pitchScalar = Mathf.Pow(1.05946f, startingNote + 2);
        notes[1].pitch = pitchScalar;

        if (songKey == Music.Key.major)
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 4);
        }
        else
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 3);
        }
        notes[2].pitch = pitchScalar;

        pitchScalar = Mathf.Pow(1.05946f, startingNote + 5);
        notes[3].pitch = pitchScalar;

        pitchScalar = Mathf.Pow(1.05946f, startingNote + 7);
        notes[4].pitch = pitchScalar;

        if (songKey == Music.Key.major)
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 9);
        }
        else
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 8);
        }
        notes[5].pitch = pitchScalar;

        if (songKey == Music.Key.major)
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 11);
        }
        else
        {
            pitchScalar = Mathf.Pow(1.05946f, startingNote + 10);
        }
        notes[6].pitch = pitchScalar;

        pitchScalar = Mathf.Pow(1.05946f, startingNote + 12);
        notes[7].pitch = pitchScalar;
    }
	
	// Update is called once per frame
	void Update () {
        if (stats.GetPlayerAlive() != true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            notes[0].loop = true;
            notes[0].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            notes[0].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            notes[1].loop = true;
            notes[1].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            notes[1].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            notes[2].loop = true;
            notes[2].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.O))
        {
            notes[2].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            notes[3].loop = true;
            notes[3].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            notes[3].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            notes[4].loop = true;
            notes[4].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            notes[4].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            notes[5].loop = true;
            notes[5].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            notes[5].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            notes[6].loop = true;
            notes[6].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            notes[6].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            notes[7].loop = true;
            notes[7].Play();
            keyQueue.NewNotePlayed(startingNote);
        }
        else if (Input.GetKeyUp(KeyCode.Semicolon))
        {
            notes[7].loop = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            lastThreeNotes = keyQueue.GetNotes();
        }
    }  
    
}
