using System;
using UnityEngine;

public class Hat : MonoBehaviour
{
    private Transform player;
    protected bool equipped = false;
    private Rigidbody playerBody;
    public float heightMultiplier = 0.35f;
    public float forwardMultiplier = 0.25f;
    public float sideOffset = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !equipped)
        {
            player = other.gameObject.GetComponent<HatWearer>().PutOnHat(this);
            playerBody = other.gameObject.GetComponent<Rigidbody>();
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
        transform.position = Vector3.Lerp(transform.position, (player.position + Vector3.up * heightMultiplier + transform.right * sideOffset) + (transform.forward * forwardMultiplier), 20f * Time.fixedDeltaTime);
    }
}
