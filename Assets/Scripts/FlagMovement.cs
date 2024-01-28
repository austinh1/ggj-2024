using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMovement : MonoBehaviour
{
    public bool enabled;
    public Rigidbody player;
    public Collider flagCollider;
    public Transform flag;

    [Range(0,20)]
    public int movements;
    [Range(1.0f,20.0f)]
    public float movementDistance;
    private int timesMoved = 0;

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (++timesMoved < movements)//needs to add the logic for being within the range
            {
                moveFlag();
            }
        }
    }

    void moveFlag()
    {
        //movement code goes here
        
    }
}
