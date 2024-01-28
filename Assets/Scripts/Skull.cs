using UnityEngine;

public class Skull : MonoBehaviour
{
    public Transform player;

    private void FixedUpdate()
    {
        var directionToPlayer = transform.position - player.position;
        transform.forward = Vector3.Lerp(transform.forward, directionToPlayer, 20f * Time.fixedDeltaTime);
    }
}
