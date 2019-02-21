using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float dampTime;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    public Transform target;
    private MovementPhysics targetPhysics;

    public float lerpTime = 1f;
    float currentLerpTime;

    public float yOffset;
    public float orthographicOffset;
    [SerializeField]
    private float yZone;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetPhysics = target.GetComponent<MovementPhysics>();
    }

    private void FixedUpdate()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        
        float yPos = Mathf.Floor((Mathf.Round(target.transform.position.y * 2) / 2 + cam.orthographicSize) / 8.75f) * 8.75f;

        float xPos = target.transform.position.x;


        transform.position = new Vector3(target.transform.position.x, Mathf.Lerp(transform.position.y, yPos, perc) - yOffset, -10);

    }
}