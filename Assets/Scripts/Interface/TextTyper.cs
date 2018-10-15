using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextTyper : MonoBehaviour {

    public float letterPause = 0.2f;
    public AudioClip typeSound1;
    public AudioClip typeSound2;

    string message;
    TextMesh textComp;

    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<TextMesh>();
        message = textComp.text;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }

}
