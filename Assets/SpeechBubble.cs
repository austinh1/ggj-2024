using System;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public Transform ball;
    public Transform cameraAnchor;
    private void FixedUpdate()
    {
        transform.position = ball.transform.position;
        transform.eulerAngles = cameraAnchor.eulerAngles;
    }
}
