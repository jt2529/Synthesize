using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class InstrumentList : MonoBehaviour {

    // Use this for initialization
    void Start () {
        // Construct the system path of the asset folder 
        string dataPath = Application.dataPath;
        string folderPath = dataPath + "/Audio/synths";
        // get the system file paths of all the files in the asset folder
        string[] filePaths = Directory.GetFiles(folderPath);
        // enumerate through the list of files loading the assets they represent and getting their type

        foreach (string filePath in filePaths)
        {
            if (filePath.Substring(filePath.Length - 4) != ".wav") { continue; }
            string assetPath = filePath.Substring(dataPath.Length - 6);
            Debug.Log(filePath);
            Debug.Log(assetPath);

            Object objAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object));

            Debug.Log(objAsset.GetType().Name);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

}
