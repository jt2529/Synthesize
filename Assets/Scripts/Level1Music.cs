using UnityEngine;
using System.Collections;

public class Level1Music : MonoBehaviour {

    public enum Key { major, minor };
    public Key songKey;
    public int noteKey;

    //Use this to set variable values
    private void Awake()
    {
        songKey = Key.major;
        noteKey = 5;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public Key getSongKey() {
        return songKey;
    }

    public int getNoteKey() {
        return noteKey;
    }
}
