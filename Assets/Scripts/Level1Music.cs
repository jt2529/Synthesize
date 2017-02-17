using UnityEngine;
using System.Collections;

public class Level1Music : MonoBehaviour {

    public enum Key { major, minor };
    Key songKey;
    int startingNote;

	// Use this for initialization
	void Start () {
        songKey = Key.major;
        startingNote = 5;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public Key getSongKey() {
        return songKey;
    }

    public int getStartingNote() {
        return startingNote;
    }
}
