using UnityEngine;
using System.Collections;
using Cinemachine;
using Synthesize.CoreSystem;

public class CameraController : MonoBehaviour
{

    public float dampTime;
    private Vector3 velocity = Vector3.zero;
    public CinemachineVirtualCamera cam;
    public PlayerStats target;
    private MovementPhysics targetPhysics;
    public float lerpTime = 1f;
    float currentLerpTime;

    public float yOffset;
    public float xOffset;
    public float orthographicOffset;

    private void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        targetPhysics = target.Core.GetCoreComponent<MovementPhysics>();
    }

    private void FixedUpdate()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPhysics.facingDirection > 0)
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(xOffset, yOffset, 0);//new Vector3(target.transform.position.x, Mathf.Lerp(transform.position.y, yPos, perc) - yOffset, -10);
        }
        else 
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(xOffset * -1, yOffset, 0);//new Vector3(target.transform.position.x, Mathf.Lerp(transform.position.y, yPos, perc) - yOffset, -10);
        }

    }
}