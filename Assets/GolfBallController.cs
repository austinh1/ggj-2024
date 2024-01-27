using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public Rigidbody body;
    public float upwardVelocity = .25f;
    public float forwardVelocity = 8f;
    public float leftRotateSpeed = -1f;
    public float rightRotateSpeed = 1f;
    
    void Start()
    {
        
    }

    void Update()
    {
        var grounded = Physics.Raycast(transform.position, Vector3.down, 1f);
        if (grounded && Input.GetMouseButtonDown(0))
        {
            body.AddForce((transform.forward + new Vector3(0, upwardVelocity, 0)) * forwardVelocity, ForceMode.Impulse);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles += new Vector3(0, leftRotateSpeed, 0);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, rightRotateSpeed, 0);
        }
    }
}
