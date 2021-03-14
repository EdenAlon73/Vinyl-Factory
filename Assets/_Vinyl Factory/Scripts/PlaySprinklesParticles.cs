using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySprinklesParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem sprinklesParticles;
    [SerializeField] private bool playParticle;
    private float yValue;

    private void Awake()
    {
        yValue = transform.position.y;
        sprinklesParticles.Stop();
    }

    private void Update()
    {
        SetParticleBool();
        PlayParticle();
    }

    private void SetParticleBool()
    {
        yValue = transform.position.y;
        print(yValue);
        
        if (yValue >= 7)
        {
            playParticle = false;
        }
        else
        {
            playParticle = true;
        }
    }

    private void PlayParticle()
    {
        if (playParticle)
        {
            sprinklesParticles.Play();
        }
        else
        {
            sprinklesParticles.Stop();
        }

    }
}
