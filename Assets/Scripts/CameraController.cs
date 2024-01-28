using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform ball;
    public Transform camera;
    public float lerpToAnchorSpeed = .9f;
    public float rotateSensitivity = 1f;
    public float gamepadRotateSensitivity = 0.1f;

    public Camera cameraLeft;
    public Camera cameraRight;

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
        transform.eulerAngles = newAngles;
        
        var orangatangAngle = new Vector3(299.72f, 84.95f, 0f);
        if (Vector3.Distance(newAngles, orangatangAngle) < 20f)
        {
            var orangObjective = ObjectiveController.Instance().GetObjective(ObjectiveType.Orangatang);
            if (!orangObjective.IsComplete)
            {
                orangObjective.Increment();
                AudioManager.Instance().PlayAudioClip("orangutan");
            }
        }
    }
    
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, ball.transform.position, lerpToAnchorSpeed * Time.fixedDeltaTime);
        // anchor.transform.position = transform.position;
    }

    public void IsThisVR() {
        cameraLeft.rect = new Rect(0, 0, 0.5f, 1);
        cameraRight.gameObject.SetActive(true);       
    }
}
