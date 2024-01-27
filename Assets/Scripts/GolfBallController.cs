using System;
using UnityEngine;

public class GolfBallController : MonoBehaviour
{
    public Rigidbody body;
    public Transform anchor;
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

    private bool isBige;
    private Vector3 lerpToScale;

    public GameObject Spring;
    private Collider springCollider;

    void Start()
    {
        UI = GameObject.FindWithTag("UI");
        lerpToScale = Vector3.one;
        springCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
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

        if (Input.GetButtonDown("Jump"))
        {
            body.drag = 2;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            body.drag = 0;
        }
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
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Sticky":
                body.drag = 2f;
                body.angularDrag = 30;
                break;
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
                break;
            case "Food":
                isBige = true;
                lerpToScale *= 1.5f;
                body.mass *= 1.25f;
                groundRaycastDistance *= 1.5f;
                Destroy(other.gameObject);
                break;
            case "MovieCamera":
                body.AddForce((Camera.main.transform.position - transform.position) * 10f, ForceMode.Impulse);
                Invoke("StopBall", 0.25f);
                break;
            case "Spring":
                Spring.SetActive(true);
                springCollider.enabled = true;
                other.gameObject.SetActive(false);
                break;
        }
    }

    void StopBall()
    {
        body.velocity = Vector3.zero;
    }
}
