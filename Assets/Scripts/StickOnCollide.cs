using UnityEngine;

public class StickOnCollide : MonoBehaviour
{
    private Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!rigid.isKinematic)
        {
            transform.SetParent(other.transform);
            rigid.isKinematic = true;
        }
    }
}
