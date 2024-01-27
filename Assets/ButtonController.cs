using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject spawnObject;
    public BoxCollider spawnPlane;

    [Range(1, 600)]
    public int spawnInterval = 15;
    [Range(1, 1000)]
    public int spawnBatchSize = 1;
    [Range(1, 1000)]
    public int spawnBatchCount = 100;
    [Range(0f, 600f)]
    public float despawnTime = 10f;

    private Transform button;
    private Vector3 pressedPos;
    private const float pushSpeed = 1f;
    private bool pressed = false;
    private int spawnTimer = 0;
    private float spawnMinX = 0f;
    private float spawnMaxX = 0f;
    private float spawnMinZ = 0f;
    private float spawnMaxZ = 0f;


    // Start is called before the first frame update
    void Start()
    {
        button = transform.Find("Button");
        pressedPos = button.position + new Vector3(0f, -0.18f, 0f);

        // We only have to calculate this once, so do it immediately when the instance is created
        var spawnWorldPos = spawnPlane.transform.position;
        var extents = spawnPlane.bounds.extents;
        spawnMinX = spawnWorldPos.x - extents.x;
        spawnMaxX = spawnWorldPos.x + extents.x;
        spawnMinZ = spawnWorldPos.z - extents.z;
        spawnMaxZ = spawnWorldPos.z + extents.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed && !button.transform.position.Equals(pressedPos))
        {
            float step = pushSpeed * Time.deltaTime;
            button.transform.position = Vector3.MoveTowards(button.transform.position, pressedPos, step);
        }
    }

    private void FixedUpdate()
    {
        if (!pressed || spawnBatchCount <= 0)
        {
            return;
        }

        spawnTimer--;

        if (spawnTimer <= 0)
        {
            for (var i = 0; i < spawnBatchSize; i++)
            {
                SpawnObject();
            }

            // Decrement the spawns remaining
            spawnBatchCount--;
            if (spawnBatchCount > 0)
            {
                spawnTimer = spawnInterval;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pressed = true;
            spawnTimer = spawnInterval;
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
