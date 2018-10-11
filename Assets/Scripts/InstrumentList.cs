using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentList : MonoBehaviour {

    string dataPath;
    List<string> listOfFiles;
    List<string> filePaths;
    Dropdown instruments;
    public Keytar keytar;
    AudioSource[] audioSources;

    // Use this for initialization
    void Start () {
        listOfFiles = new List<string>();
        filePaths = new List<string>();
        instruments = GetComponent<Dropdown>();
        audioSources = keytar.GetComponents<AudioSource>();

        // Construct the system path of the asset folder 
        dataPath = Application.dataPath;
        string folderPath = dataPath + "/Audio/synths";
        // get the system file paths of all the files in the asset folder
        string[] allFilePaths = Directory.GetFiles(folderPath);
        // enumerate through the list of files loading the assets they represent and getting their type
        

        foreach (string filePath in allFilePaths)
        {
            if (filePath.Substring(filePath.Length - 4) != ".wav") { continue; }
            int pos = filePath.LastIndexOf("\\") + 1;
            listOfFiles.Add(filePath.Substring(pos, filePath.Length - pos));
            filePaths.Add(filePath);
        }
        instruments.AddOptions(listOfFiles);
    }

    public void LoadInstrument()
    {
        string filePath = filePaths[instruments.value];
        string assetPath = filePath.Substring(dataPath.Length - 6);
        Object objAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.clip = (AudioClip)objAsset;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

}
