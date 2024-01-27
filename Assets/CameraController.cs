using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSensitivity;
    
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var newAngles = transform.eulerAngles + new Vector3(-rotateSensitivity * Input.GetAxis("Mouse Y"), rotateSensitivity * Input.GetAxis("Mouse X"), 0);
        newAngles = new Vector3(Mathf.Clamp(newAngles.x, 0f, 90f), newAngles.y, newAngles.z);
        Debug.Log(newAngles);
        transform.eulerAngles = newAngles;
    }
}
