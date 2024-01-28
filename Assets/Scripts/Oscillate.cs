using System;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [Range(-360f, 360f)]
    public float minRotate = -10f;
    [Range(-360f, 360f)]
    public float maxRotate = 10f;
    [Range(1f, 10f)]
    public float rotateSpeed = 1f;

    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    void Update()
    {
        float oscillation = Mathf.Sin(Time.time * rotateSpeed);

        var newX = rotateX ? Mathf.Lerp(minRotate, maxRotate, (oscillation + 1f) / 2f) : transform.rotation.x;
        var newY = rotateY ? Mathf.Lerp(minRotate, maxRotate, (oscillation + 1f) / 2f) : transform.rotation.y;
        var newZ = rotateZ ? Mathf.Lerp(minRotate, maxRotate, (oscillation + 1f) / 2f) : transform.rotation.z;

        transform.rotation = Quaternion.Euler(newX, newY, newZ);
    }
}
