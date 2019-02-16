using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBoxController : MonoBehaviour {

    private MovementPhysics physics;
    private Vector3 velocity;

    [SerializeField]
    float gravity;

    // Use this for initialization
    void Start () {
        physics = GetComponent<MovementPhysics>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {

        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
        }

        velocity.y += 5 * Time.deltaTime;
        physics.Move(velocity * Time.deltaTime);
        if (physics.collisions.left || physics.collisions.right)
        {

        }
    }
}
