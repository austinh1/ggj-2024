using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GolfBallController : MonoBehaviour
{
    public Rigidbody body;
    public Transform anchor;
    public float upwardVelocity = .25f;
    public float forwardVelocity = 8f;
    [Range(0.01f, 0.1f)]
    public float swingRate = 0.01f;
    public float groundRaycastDistance = 1f;
    public float slowmoDuration = 10f;
    public float resetHoldDuration = 10f;
    public GameObject explosionPrefab;
    public MeshRenderer meshRenderer;
    public Camera cameraLeft;
    public Camera cameraRight;

    [HideInInspector]
    public bool prepSwing = false;
    [HideInInspector]
    public float swingStrength = 0f;

    private int strengthBarDir = 1;
    private int missTimer = 0;
    private GameObject UI;
    private int strokes = 0;

    private bool isBige;
    private Vector3 lerpToScale;

    public GameObject Spring;
    private Collider springCollider;
    private float slowmoStartTime;

    bool stuck;
    private float stuckTimer;
    private float stuckWaitTime = 3f;
    private Vector3 lastPos;

    private float rightClickHoldTime;
    private Vector3 startPosition;

    public CameraController camController;
    private ObjectiveController objectiveController;
    private AudioManager audioManager;

    private float timeLastGrounded;
    public AudioSource screamAudio;

    void Start()
    {
        UI = GameObject.FindWithTag("UI");
        lerpToScale = Vector3.one;
        springCollider = GetComponent<CapsuleCollider>();

        var position = transform.position;
        lastPos = position;
        startPosition = position;

        objectiveController = ObjectiveController.Instance();
        audioManager = AudioManager.Instance();
    }

    void Update()
    {
        if (!meshRenderer.enabled)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        
        var grounded = Physics.Raycast(transform.position, Vector3.down, groundRaycastDistance);
        if (grounded)
        {
            timeLastGrounded = Time.time;
            if (screamAudio.isPlaying)
            {
                screamAudio.Stop();
            }
        }

        CheckIfStuck(grounded);
        
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            prepSwing = true;
            strengthBarDir = 1;
            swingStrength = 0f;
        }
        if (prepSwing && (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1")))
        {
            // Only allow hitting if the ball is currently grounded
            if (grounded || stuck)
            {
                body.AddForce((anchor.forward + new Vector3(0, upwardVelocity * swingStrength * (springCollider.enabled ? 1.5f : 1f), 0)) * (forwardVelocity * swingStrength), ForceMode.Impulse);
                body.AddTorque(anchor.right * 10f, ForceMode.Impulse);
                stuck = false;

                if (swingStrength == 1f)
                {
                    objectiveController.GetObjective(ObjectiveType.MaxSwing).Increment();
                }

                if (swingStrength >= 0.9f)
                {
                    audioManager.PlayAudioClip("move - big hit");
                }
                else if (swingStrength >= 0.4f)
                {
                    audioManager.PlayAudioClip("move - medium hit");
                }
                else
                {
                    string[] sfxOptions = { "move - small hit 1", "move - small hit 2" };
                    var choice = UnityEngine.Random.Range(0, sfxOptions.Length);
                    audioManager.PlayAudioClip(sfxOptions[choice]);
                }
            }
            else
            {
                MissSwing();
            }

            swingStrength = 0f;
            prepSwing = false;
            strokes++;

            if (strokes == 1)
            {
                objectiveController.GetObjective(ObjectiveType.Fore).Increment();
            }

            var strokesText = UI.transform.Find("Strokes").GetComponent<TMPro.TextMeshProUGUI>();
            strokesText.text = string.Format("Strokes: {0}", strokes);
        }

        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Fire2"))
        {
            var objective = objectiveController.GetObjective(ObjectiveType.RightClickToBrake);
            if (!objective.IsComplete)
            {
                objective.Increment();
            }
            body.drag = 2;
            rightClickHoldTime = 0;
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Fire2"))
        {
            body.drag = 0;
            rightClickHoldTime = 0;
        }

        if (Input.GetMouseButton(1) || Input.GetButton("Fire2"))
        {
            rightClickHoldTime += Time.deltaTime;

            if (gameObject.activeSelf && rightClickHoldTime >= resetHoldDuration)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                audioManager.PlayAudioClip("reset - explosion");

                rightClickHoldTime = 0;
                meshRenderer.enabled = false;
                body.velocity = Vector3.zero;
                body.useGravity = false;
                isBige = false;
                lerpToScale = Vector3.one;
                transform.localScale = Vector3.one;
                Time.timeScale = 1f;
                body.drag = 0f;
                cameraLeft.rect = new Rect(0, 0, 1f, 1);
                cameraRight.gameObject.SetActive(false);

                foreach (var hat in GetComponent<HatWearer>().hats)
                {
                    Destroy(hat);
                }
                
                Spring.SetActive(false);
                springCollider.enabled = false;
                StartCoroutine(ResetAfterExplosionWithDelay(1f));
            }
        }

        if (Time.time - slowmoStartTime >= slowmoDuration && Time.timeScale < 1f)
        {
            Time.timeScale = 1f;
        }

        if (Time.time - timeLastGrounded > 5f && !screamAudio.isPlaying)
        {
            screamAudio.Play();
        }
        
        lastPos = transform.position;
    }

    private IEnumerator ResetAfterExplosionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        meshRenderer.enabled = true;
        body.useGravity = true;
        transform.position = startPosition;
        objectiveController.GetObjective(ObjectiveType.Reset).Increment();
    }

    private void FixedUpdate()
    {
        if (prepSwing)
        {
            swingStrength = Math.Clamp(swingStrength + swingRate * strengthBarDir, 0f, 1f);
            if (swingStrength == 0f || swingStrength == 1f)
            {
                strengthBarDir = -strengthBarDir;
            }
        }

        if (isBige && transform.localScale != lerpToScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, lerpToScale, 2f * Time.fixedDeltaTime);
        }
        // else
        // {
        //     transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.fixedDeltaTime);
        // }

        TickTimers();
    }

    private void TickTimers()
    {
        if (missTimer > 0)
        {
            missTimer--;
            if (missTimer == 0)
            {
                var missText = UI.transform.Find("Miss");
                missText.gameObject.SetActive(false);
            }
        }
    }

    private void MissSwing()
    {
        var missText = UI.transform.Find("Miss");
        missText.gameObject.SetActive(true);
        missTimer = 45;
        audioManager.PlayAudioClip("move - whiff");
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Sticky":
                body.drag = 2f;
                body.angularDrag = 30;
                objectiveController.GetObjective(ObjectiveType.StickyPizza).Increment();
                audioManager.PlayAudioClip("pizza - landing on");
                break;
            case "Trophy":
            {
                var trophyObjective = objectiveController.GetObjective(ObjectiveType.ObstacleCourse);
                if (!trophyObjective.IsComplete)
                {
                    trophyObjective.Increment();
                    audioManager.PlayAudioClip("objective-Trophy");
                }
                string[] sfxOptions = { "trophy - clang 1", "trophy - clang 2" };
                var choice = UnityEngine.Random.Range(0, sfxOptions.Length);
                audioManager.PlayAudioClip(sfxOptions[choice]);
                break;
            }
            case "Pin":
            {
                objectiveController.GetObjective(ObjectiveType.Bowling).Increment();
                audioManager.PlayAudioClip("bowling pin");
                break;
            }
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Sticky":
                body.drag = .1f;
                body.angularDrag = .1f;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Hole":
                body.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                audioManager.PlayAudioClip("hole - gotcha");
                break;
            case "Food":
                isBige = true;
                lerpToScale *= 1.5f;
                body.mass *= 1.25f;
                groundRaycastDistance *= 1.5f;
                Destroy(other.gameObject);
                audioManager.PlayAudioClip("taco - crunch");
                break;
            case "MovieCamera":
                body.AddForce((Camera.main.transform.position - transform.position) * 10f, ForceMode.Impulse);
                Invoke("StopBall", 0.25f);
                objectiveController.GetObjective(ObjectiveType.MovieCamera).Increment();
                audioManager.PlayAudioClip("camera - breaking glass");
                break;
            case "Spring":
                Spring.SetActive(true);
                springCollider.enabled = true;
                Destroy(other.gameObject);
                objectiveController.GetObjective(ObjectiveType.Spring).Increment();
                audioManager.PlayAudioClip("spring - boing");
                break;
            case "Slowmo":
                Destroy(other.gameObject);
                slowmoStartTime = Time.time;
                Time.timeScale = .5f;
                objectiveController.GetObjective(ObjectiveType.Slowmo).Increment();
                break;
            case "VRHeadset":
                camController.IsThisVR();
                objectiveController.GetObjective(ObjectiveType.VR).Increment();
                audioManager.PlayAudioClip("vr");
                break;
            case "MiddleOfDonut":
                objectiveController.GetObjective(ObjectiveType.Donut).Increment();
                audioManager.PlayAudioClip("donut - thunk");
                break;
        }
    }

    void StopBall()
    {
        body.velocity = Vector3.zero;
    }

    void CheckIfStuck(bool isGrounded)
    {
        Vector3 currentPos = transform.position;
        
        if (!isGrounded)
        {
            if ((currentPos - lastPos).magnitude == 0)
            {
                stuckTimer += Time.deltaTime;
                
                if (stuckTimer > stuckWaitTime)
                {
                    stuck = true;
                    stuckTimer = 0f;
                }
            }
            else
            {
                stuckTimer = 0f;
            }
        }
    }
}
