using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UmmAPI;
using Newtonsoft.Json.Linq;

public class CustomRocketLogic : MonoBehaviour
{
    void Start()
    {
        gameObject.layer = 9;
        if (!GetComponent<Rigidbody2D>())
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected!");
        if ((other.transform.GetComponent<Rigidbody2D>() && other.transform.gameObject.layer == 9) || (other.transform.GetComponent<Rigidbody2D>() && other.transform.gameObject.layer == 11))
        {
            Explode();
        }

    }
    void Explode()
    {
        Debug.Log("Conditions met. Performing actions...");

        foreach (ParticleSystem Particles in transform.transform.GetComponentsInChildren<ParticleSystem>())
        {
            Particles.transform.SetParent(null);
            Particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Particles.gameObject.AddComponent<DeleteAfterTime>().Life = 5f;
        }
        transform.GetComponent<LaunchedRocketBehaviour>().Explosion.Position = transform.position;
        ExplosionCreator.CreateFragmentationExplosion(transform.GetComponent<LaunchedRocketBehaviour>().Explosion);
        Destroy(gameObject);
    }
}