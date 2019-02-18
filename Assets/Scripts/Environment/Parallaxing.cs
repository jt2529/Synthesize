using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;		//array (list) of all back- and forgrounds to be parallaxed
	public float[] parallaxScales;	//the proportion of the camera's movement to move the backgrounds by
	public float smoothing = 1f;	//how smooth the parallax is going to be, Must be above 0 otherwize the parallax will not work

	public Transform cam;	//reference to the camera's transform
	private Vector3 previousCamPos;		//the position of the camera in the previous frame

	//called before Start(), using to assign references.
	void Awake() {
		//set up camera the reference
	}

	// Use this for initialization
	void Start () {
		// store previous frame
		previousCamPos = cam.position;

		//declares the length of the array
		parallaxScales = new float[backgrounds.Length];

		//assigning coresponding parallaxScales
		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z * -1;
		}
	}

	// Update is called once per frame
	void Update() {

		//for each background

		for (int i = 0; i < backgrounds.Length; i++) {
			//the parallax is the opposite of the camera movement because the previous frame multiplied by the scale
			float parallax =  (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set a target x position that is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			//create a target position which is the backgrounds current position with it's target x position
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			//fade batween current position and the target position using lerp
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

		}

		//set the previousCamPos to the camera's position at the end of the frame
		previousCamPos = cam.position;

	}
}

/*

Code written by joedanhol, find this on GitHub at https://github.com/joedanhol/Parallax2D :D Last test done 27/04/15
Feel free to edit this at your own will, this code was made to be compleatly hackable, and feel free to message me at
joedanhol@gmail.com for any help required.

*/
