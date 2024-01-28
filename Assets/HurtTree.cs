using UnityEngine;

public class HurtTree : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.rigidbody.AddForce((other.transform.position - transform.position).normalized * 12f, ForceMode.Impulse);
        }
    }
}
