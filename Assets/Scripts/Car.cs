using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public List<Rigidbody> policeCars;
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
            
            transform.forward =
                Vector3.Lerp(transform.forward, forward, 20f * Time.deltaTime);

            foreach (var policeCar in policeCars)
            {
                policeCar.AddForce((transform.position - policeCar.position).normalized * 40f); 
                var policeVel = Mathf.Min(policeCar.velocity.magnitude, 100f);
                policeCar.velocity = policeCar.velocity.normalized * policeVel;

                policeCar.transform.forward =
                    Vector3.Lerp(transform.forward, policeCar.velocity.normalized, 20f * Time.deltaTime);
            }
            
            if (transform.position.magnitude > 1000)
            {
                Destroy(gameObject);
                foreach (var car in policeCars)
                {
                    Destroy(car.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            truckin = true;

            foreach (var policeCar in policeCars)
            {
                policeCar.gameObject.SetActive(true);
            }
        }
    }
}
