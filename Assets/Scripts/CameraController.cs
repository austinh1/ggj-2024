using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ball;
    public float lerpToAnchorSpeed = .9f;
    public float rotateSensitivity = 1f;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = ball.transform.position;
    }

    void Update()
    {
        var newAngles = transform.eulerAngles + new Vector3(-rotateSensitivity * Input.GetAxis("Mouse Y"), rotateSensitivity * Input.GetAxis("Mouse X"), 0);
        newAngles = new Vector3(Mathf.Clamp(newAngles.x, 0f, 90f), newAngles.y, newAngles.z);
        transform.eulerAngles = newAngles;
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, ball.transform.position, lerpToAnchorSpeed * Time.fixedDeltaTime);
        // anchor.transform.position = transform.position;
    }
}
