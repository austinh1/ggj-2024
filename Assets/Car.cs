using UnityEngine;

public class Car : MonoBehaviour
{
    public Rigidbody body;

    private bool truckin = false;
    private Vector3 forward;

    private void Start()
    {
        forward = transform.forward;
    }
    
    private void Update()
    {
        if (truckin)
        {
            body.AddForce(forward * 50f);
            var vel = Mathf.Min(body.velocity.magnitude, 100f);
            body.velocity = body.velocity.normalized * vel;

            if (transform.position.magnitude > 1000)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            truckin = true;
        }
    }
}
