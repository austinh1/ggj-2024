using UnityEngine;

public class ObjectiveOnTrigger : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && (repeatable || !done))
        {
            objective.Increment(progress);
            done = true;
        }
    }
}
