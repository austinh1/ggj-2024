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
    
    private void FixedUpdate()
    {
        if (truckin)
        {
            body.AddForce(forward.normalized * 50f, ForceMode.Acceleration);
            
            transform.forward =
                Vector3.Lerp(transform.forward, forward, 20f * Time.deltaTime);

            foreach (var policeCar in policeCars)
            {
                policeCar.AddForce((transform.position - policeCar.position).normalized * 40f, ForceMode.Acceleration); 
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
            AudioManager.Instance().PlayAudioClip("police - whoop");
            truckin = true;

            foreach (var policeCar in policeCars)
            {
                policeCar.gameObject.SetActive(true);
            }
        }
    }
}
