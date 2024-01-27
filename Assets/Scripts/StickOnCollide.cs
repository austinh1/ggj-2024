using UnityEngine;

public class StickOnCollide : MonoBehaviour
{
    private Rigidbody rigid;

    void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (rigid != null && !rigid.isKinematic)
        {
            transform.SetParent(other.transform, true);
            rigid.isKinematic = true;
            rigid.useGravity = false;
        }
    }
}
