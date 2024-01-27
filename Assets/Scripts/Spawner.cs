using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public BoxCollider spawnPlane;

    public bool infinite = false;
    public bool active = true;

    [Range(1, 600)]
    public int spawnInterval = 10;
    [Range(1, 1000)]
    public int spawnBatchSize = 10;
    [Range(1, 1000)]
    public int spawnBatchCount = 100;
    [Range(0f, 600f)]
    public float despawnTime = 10f;
    
    private int spawnTimer = 0;
    private float spawnMinX = 0f;
    private float spawnMaxX = 0f;
    private float spawnMinZ = 0f;
    private float spawnMaxZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // We only have to calculate this once, so do it immediately when the instance is created
        var spawnWorldPos = spawnPlane.transform.position;
        var extents = spawnPlane.bounds.extents;
        spawnMinX = spawnWorldPos.x - extents.x;
        spawnMaxX = spawnWorldPos.x + extents.x;
        spawnMinZ = spawnWorldPos.z - extents.z;
        spawnMaxZ = spawnWorldPos.z + extents.z;
    }

    private void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        if (spawnBatchCount <= 0 && !infinite)
        {
            Destroy(GetComponent<Spawner>());
            return;
        }

        spawnTimer--;

        if (spawnTimer <= 0)
        {
            for (var i = 0; i < spawnBatchSize; i++)
            {
                SpawnObject();
            }

            if (!infinite)
            {
                // Decrement the spawns remaining
                spawnBatchCount--;
            }

            if (spawnBatchCount > 0)
            {
                spawnTimer = spawnInterval;
            }
        }
    }

    private void SpawnObject()
    {
        var spawnX = Random.Range(spawnMinX, spawnMaxX);
        var spawnZ = Random.Range(spawnMinZ, spawnMaxZ);
        var spawnPos = new Vector3(spawnX, spawnPlane.transform.position.y, spawnZ);

        var newObject = Instantiate(spawnObject, spawnPos, Quaternion.identity);
        var timerScript = newObject.AddComponent<DespawnTimer>();
        timerScript.timeSeconds = despawnTime;
    }
}
