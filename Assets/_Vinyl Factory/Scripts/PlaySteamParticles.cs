using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySteamParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem steamParticles;
    [SerializeField] private bool playParticle;
    
    
    private float yValue;

    private void Start()
    {
        yValue = transform.position.y;
        steamParticles.Stop();
    }

    private void Update()
    {
        SetParticleBool();
        PlayParticle();
    }

    private void SetParticleBool()
    {
        yValue = transform.position.y;

        if (yValue >= 1.61f)
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
            steamParticles.Play();
        }
        else
        {
            steamParticles.Stop();
        }

    }
}
