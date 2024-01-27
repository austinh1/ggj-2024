using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupVrHeadset : MonoBehaviour
{
    public Camera cameraLeft;
    public Camera cameraRight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cameraLeft.rect = new Rect(0, 0, 0.5f, 1);
            cameraRight.gameObject.SetActive(true);
        }
    }
}
