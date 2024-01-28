using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatWearer : MonoBehaviour
{
    private GameObject topMostHat;

    // returns transform of previous topMostHat
    public Transform PutOnHat(Hat hat) {
        Transform prevTop;
        if (topMostHat == null) {
            prevTop = transform;
        } else {
            prevTop = topMostHat.transform;
        }
        topMostHat = hat.gameObject;
        return prevTop;
    }
}
