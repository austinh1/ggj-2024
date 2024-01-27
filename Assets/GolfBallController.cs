using System;
using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public Rigidbody body;
    public GameObject anchor;
    public float upwardVelocity = .25f;
    public float forwardVelocity = 8f;
    public float leftRotateSpeed = -1f;
    public float rightRotateSpeed = 1f;
    [Range(0.001f, 0.01f)]
    public float swingRate = 0.001f;

    [HideInInspector]
    public bool prepSwing = false;
    [HideInInspector]
    public float swingStrength = 0f;
    
    private int strengthBarDir = 1;
    
    void Start()
    {
        
    }

    void Update()
    {
        var grounded = Physics.Raycast(transform.position, Vector3.down, 1f);
        if (grounded && Input.GetMouseButtonDown(0))
        {
            prepSwing = true;
            strengthBarDir = 1;
            swingStrength = 0f;
        }
        if (prepSwing)
        {
            if (Input.GetMouseButtonUp(0))
            {
                body.AddForce((transform.forward + new Vector3(0, upwardVelocity, 0)) * forwardVelocity * swingStrength, ForceMode.Impulse);
                swingStrength = 0f;
                prepSwing = false;
            }
            else
            {
                swingStrength = Math.Clamp(swingStrength + swingRate * strengthBarDir, 0f, 1f);
                if (swingStrength == 0f || swingStrength == 1f)
                {
                    strengthBarDir = -strengthBarDir;
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            anchor.transform.eulerAngles += new Vector3(0, leftRotateSpeed, 0);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            anchor.transform.eulerAngles += new Vector3(0, rightRotateSpeed, 0);
        } 
    }

    private void LateUpdate() {
        anchor.transform.position = transform.position;
    }
}
