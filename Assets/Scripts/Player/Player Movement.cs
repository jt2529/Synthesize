using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    MovementPhysics physics;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        physics = GetComponent<MovementPhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
            if (physics.collisions.above)
            {
                //forceUpward = 0;
            }
        }
    }
}
