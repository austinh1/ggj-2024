using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public Rigidbody body;
    public float bounciness = 5f;
    private bool bouncing = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" || bouncing)
        {
            var contact = other.contacts[0];
            bouncing = true;
            var dir = transform.position - contact.point;
            body.AddForce(dir * bounciness, ForceMode.Impulse);
        }
    }
}
