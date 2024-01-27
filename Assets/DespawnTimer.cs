using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    [Range(0f, 600f)]
    public float timeSeconds = 10f;

    void Start()
    {
        Destroy(gameObject, timeSeconds);
    }
}
