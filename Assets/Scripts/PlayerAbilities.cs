using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

    AudioSource[] notes;

    // Use this for initialization
    void Start () {
        notes = GameObject.FindGameObjectWithTag("Keytar").GetComponents<AudioSource>();
        
	
	}
	
	// Update is called once per frame
	void Update () {

        // For the F scale (in this test) the notes will be: 7,9,10,12,14,16,17
        if (Input.GetKeyDown(KeyCode.U)) {
            notes[5].Play();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            notes[7].Play();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            notes[9].Play();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            notes[10].Play();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            notes[12].Play();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            notes[14].Play();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            notes[16].Play();
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            notes[17].Play();
        }
    }
}
