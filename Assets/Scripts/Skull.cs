using UnityEngine;

public class Skull : MonoBehaviour
{
    public Transform player;
    public float distanceToAppear = 25f;
    [HideInInspector]
    public bool appear = false;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (!appear && Vector3.Distance(transform.position, player.transform.position) < distanceToAppear)
        {
            appear = true;
            ObjectiveController.Instance().GetObjective(ObjectiveType.PumpkinForest).Increment();
        }

        if (appear && transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 5f * Time.fixedDeltaTime);
        }
        
        var directionToPlayer = transform.position - player.position;
        transform.forward = Vector3.Lerp(transform.forward, directionToPlayer, 20f * Time.fixedDeltaTime);
    }
}
