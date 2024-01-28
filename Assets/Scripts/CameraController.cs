using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ball;
    public Transform camera;
    public float lerpToAnchorSpeed = .9f;
    public float rotateSensitivity = 1f;
    public float gamepadRotateSensitivity = 0.1f;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = ball.transform.position;
    }

    void Update()
    {
        float xRotation;
        float yRotation;

        xRotation = Input.GetAxis("Vertical") != 0
            ? gamepadRotateSensitivity * Input.GetAxis("Vertical")
            : rotateSensitivity * Input.GetAxis("Mouse Y");
        yRotation = Input.GetAxis("Horizontal") != 0
            ? gamepadRotateSensitivity * Input.GetAxis("Horizontal")
            : rotateSensitivity * Input.GetAxis("Mouse X");
            
        
        var newAngles = transform.eulerAngles + new Vector3(-xRotation, yRotation, 0);
        // newAngles = new Vector3(Mathf.Clamp(newAngles.x, 0f, 90f), newAngles.y, newAngles.z);
        transform.eulerAngles = newAngles;    
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, ball.transform.position, lerpToAnchorSpeed * Time.fixedDeltaTime);
        // anchor.transform.position = transform.position;
    }
}
