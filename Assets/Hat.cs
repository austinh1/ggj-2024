using System;
using UnityEngine;

public class Hat : MonoBehaviour
{
    private Transform player;
    private bool equipped = false;
    private Rigidbody playerBody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.transform;
            playerBody = player.GetComponent<Rigidbody>();
            equipped = true;
        }
    }

    private void FixedUpdate()
    {
        if (!equipped)
        {
            return;
        }
        
        transform.forward = Vector3.Lerp(transform.forward, playerBody.velocity.normalized, 10f * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, (player.position + Vector3.up * .35f) + (transform.forward * .25f), 20f * Time.fixedDeltaTime);
    }
}
