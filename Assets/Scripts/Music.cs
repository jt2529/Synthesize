using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Music : MonoBehaviour {

    public Keytar keytar;
    public AudioSource audioSource;
    public Key songKey;
    public int noteKey;

    //Use this to set variable values
    private void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        keytar = GetComponent<Keytar>();
        keytar.ChangeKey(noteKey, songKey);
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
