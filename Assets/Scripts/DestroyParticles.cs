using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    ParticleSystem particles;


    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        if (particles)
        {
            if (!particles.IsAlive())
            {
                Destroy(gameObject);
            }
        }     
    }
}
