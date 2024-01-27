using System;
using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public Collider collider;
    public Rigidbody body;
    public Transform anchor;
    public Transform hole;
    public float upwardVelocity = .25f;
    public float forwardVelocity = 8f;
    [Range(0.01f, 0.1f)]
    public float swingRate = 0.01f;
    public float groundRaycastDistance = 1f;

    [HideInInspector]
    public bool prepSwing = false;
    [HideInInspector]
    public float swingStrength = 0f;

    private int strengthBarDir = 1;
    private int missTimer = 0;
    private GameObject UI;
    private int strokes = 0;

    private bool lerpToHole = false;

    void Start()
    {
        UI = GameObject.FindWithTag("UI");
    }

    void Update()
    {
        if (lerpToHole)
        {
            return;
        }

        var grounded = Physics.Raycast(transform.position, Vector3.down, groundRaycastDistance);
        if (Input.GetMouseButtonDown(0))
        {
            prepSwing = true;
            strengthBarDir = 1;
            swingStrength = 0f;
        }
        if (prepSwing && Input.GetMouseButtonUp(0))
        {
            // Only allow hitting if the ball is currently grounded
            if (grounded)
            {
                body.AddForce((anchor.forward + new Vector3(0, upwardVelocity * swingStrength, 0)) * (forwardVelocity * swingStrength), ForceMode.Impulse);
                body.AddTorque(anchor.right * 10f, ForceMode.Impulse);
            }
            else
            {
                MissSwing();
            }

            swingStrength = 0f;
            prepSwing = false;
            strokes++;

            var strokesText = UI.transform.Find("Strokes").GetComponent<TMPro.TextMeshProUGUI>();
            strokesText.text = string.Format("Strokes: {0}", strokes);
        }
    }

    private void FixedUpdate()
    {
        if (lerpToHole)
        {
            transform.position = Vector3.Lerp(transform.position, hole.position, .9f * Time.fixedDeltaTime);
        }

        if (prepSwing)
        {
            swingStrength = Math.Clamp(swingStrength + swingRate * strengthBarDir, 0f, 1f);
            if (swingStrength == 0f || swingStrength == 1f)
            {
                strengthBarDir = -strengthBarDir;
            }
        }

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
        missTimer = 300;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hole"))
        {
            collider.enabled = false;
            body.isKinematic = true;
            lerpToHole = true;
        }
    }
}
