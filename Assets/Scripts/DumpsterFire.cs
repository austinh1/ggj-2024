using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpsterFire : MonoBehaviour
{
    public List<ParticleSystem> fireParticles;

    private void Start()
    {
        foreach (var fireParticle in fireParticles)
        {
            var emission = fireParticle.emission;
            emission.enabled = false;
            fireParticle.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var fireParticle in fireParticles)
            {
                var emission = fireParticle.emission;
                emission.enabled = true;
                fireParticle.gameObject.SetActive(true);
            }
            
            ObjectiveController.Instance().GetObjective(ObjectiveType.DumpsterFire).Increment();
        }
    }
}
