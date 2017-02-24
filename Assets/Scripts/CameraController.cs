using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Camera cam;
    public float offsetUp;
    public float offsetRight;
    public float defaultUp;
    public float defaultRight;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultUp = .5f;
        defaultRight = .7f;
    }

    // Update is called once per frame
    void Update()
    {
        offsetUp = defaultUp;
        offsetRight = defaultRight;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (input.y > 0)
        {
            defaultUp = .7f;
        }

        if (input.y < 0)
        {
            defaultUp = -.2f;
        }

        if (input.x > 0)
        {
            defaultRight = .9f;
        }

        if (input.x < 0)
        {
            defaultRight = -.2f;
        }

        if (target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            delta.x = delta.x + offsetRight;
            delta.y = delta.y + offsetUp;
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
}