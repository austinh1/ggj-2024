using System.Collections;
using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    [Range(0f, 600f)]
    public float timeSeconds = 10f;

    private Coroutine destroyCorutine;

    void Start()
    {
        destroyCorutine = StartCoroutine(DestroyAfterDelay(timeSeconds));
    }

    // Coroutine to destroy the object after a delay
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void Cancel()
    {
        if (destroyCorutine != null)
        {
            StopCoroutine(destroyCorutine);
            // When it's cancelled, there's no way to start it again, so just remove the script
            Destroy(this);
        }
    }
}
