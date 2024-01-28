using System;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [Range(-360f, 360f)]
    public float minRotate = -10f;
    [Range(-360f, 360f)]
    public float maxRotate = 10f;
    [Range(0f, 10f)]
    public float rotateSpeed = 1f;

    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    private int rotateDir = 1;

    // Update is called once per frame
    void Update()
    {
        // Store the inital values
        var newX = transform.rotation.x;
        var newY = transform.rotation.y;
        var newZ = transform.rotation.z;

        var amount = Time.deltaTime * rotateSpeed * rotateDir;
        bool reverseDir = false;

        // Adjust the axes that are enabled
        if (rotateX)
        {
            newX = Math.Clamp(newX + amount, minRotate, maxRotate);
            if (newX == minRotate || newX == maxRotate)
            {
                reverseDir = true;
            }
        }
        if (rotateY)
        {
            newY = Math.Clamp(newY + amount, minRotate, maxRotate);
            if (newY == minRotate || newY == maxRotate)
            {
                reverseDir = true;
            }
        }
        if (rotateZ)
        {
            newZ = Math.Clamp(newZ + amount, minRotate, maxRotate);
            if (newZ == minRotate || newZ == maxRotate)
            {
                reverseDir = true;
            }
        }

        var newRotation = new Quaternion(newX, newY, newZ, transform.rotation.w);
        transform.rotation = newRotation;

        // Change the oscillating direction if we reached one of the max bounds
        if (reverseDir)
        {
            rotateDir = -rotateDir;
        }
    }
}
