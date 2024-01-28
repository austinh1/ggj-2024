using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : MonoBehaviour
{
    public ParticleSystem fire;

    private void Start()
    {
        fire.gameObject.SetActive(false);
        var emissionModule = fire.emission;
        emissionModule.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !fire.gameObject.activeSelf)
        {
            fire.gameObject.SetActive(true);
            var emissionModule = fire.emission;
            emissionModule.enabled = true;
            ObjectiveController.Instance().GetObjective(ObjectiveType.FireHydrant).Increment();
        }
    }
}
