using UnityEngine;

public class ObjectiveOnHit : MonoBehaviour
{
    public ObjectiveType objectiveType;
    public int progress = 1;
    public bool repeatable = false;

    private Objective objective;
    private bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        objective = ObjectiveController.Instance().GetObjective(objectiveType);
    }

    private void Hit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && (repeatable || !done))
        {
            objective.Increment(progress);
            done = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Hit(other.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit(other);
    }
}
