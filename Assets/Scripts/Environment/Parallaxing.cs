using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;         //List of all the back/forgrounds to be parallaxed
    private float[] parallaxScales;          //Proportion of the camera's movement to move the backgrounds by
    public float smoothing;                 //How smooth the paralax will be

    private Transform cam;                  //Reference to the main camera's transform
    private Vector3 previousCamPos;         //Position of camera from previous frame

    
    void Awake()
    {
        cam = Camera.main.transform;        
    }

    // Use this for initialization
    void Start () {
        // Previous frame had the current frame's camera position
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;

    }   
}
