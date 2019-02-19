/*
*             gillemp            -         Triunity Studios      
*   https://twitter.com/guplem  -  http://www.triunitystudios.com
*/

#pragma warning disable 0649
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraLR : MonoBehaviour
{

    private int resolutionWidith = 1;
    private int resolutionHeight = 144; //Height resolution of the screen

    private Camera cam;
    [SerializeField]
    private Camera virtualCamera; //camera that is son of the main camera
    [SerializeField]
    private GameObject virtualScreenQuad; //quad that is son of the virtual camera
    [SerializeField]
    RenderTexture defaultTexture; //Texture applied to the main camera and the material that is applied to the quad

    void Start()
    {
        //Get the camera component
        cam = GetComponent<Camera>();

        //Calculate the screen ratio
        float ratio = ((float)cam.pixelWidth / (float)cam.pixelHeight);
        //Calculate the resolution widith
        resolutionWidith = Mathf.RoundToInt(resolutionHeight * ratio);

        //Set the propoer properties for the virtual camera
        virtualCamera.orthographicSize = Mathf.RoundToInt(resolutionHeight / 2);
        //Set the propoer properties for the virtual screen quad
        virtualScreenQuad.GetComponent<Transform>().localScale = new Vector3(resolutionWidith, resolutionHeight, 1.0f);

        //Set the propoer properties for the texture
        defaultTexture.width = resolutionWidith;
        defaultTexture.height = resolutionHeight;
    }

}
