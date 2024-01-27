using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var grounded = Physics.Raycast(transform.position, Vector3.down, 5f);
        if (grounded && Input.GetMouseButtonDown(0))
        {
            rigidbody.AddForce((transform.forward + new Vector3(0, 1, 0)) * 13f, ForceMode.Impulse);
        }
    }
}
