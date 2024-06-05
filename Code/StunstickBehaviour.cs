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

public class StunstickBehaviour : MonoBehaviour
{
    public float ChargeAmount = 10;
    public float ForceNeeded = 0.2f;
    public GameObject Mask;
    public GameObject Glow;
    protected AudioSource SoundSource;
    public bool Activated;
    public AudioClip Clip;
    private static readonly ContactPoint2D[] buffer = new ContactPoint2D[8];

    void Start()
    {
        Glow = transform.Find("ModLightPrefab(Clone)").gameObject;
        Glow.SetActive(false);
        Mask.SetActive(false);
        SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.dopplerLevel = 0f;
        SoundSource.playOnAwake = false;
        SoundSource.rolloffMode = AudioRolloffMode.Linear;
        SoundSource.minDistance = 1f;
        SoundSource.maxDistance = 25f;
        SoundSource.spatialBlend = 1f;
        SoundSource.volume = 1;
        SoundSource.clip = Clip;
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
    }
    void Use(ActivationPropagation activationPropagation)
    {
        Activated = !Activated;
        if (Activated)
        {
            Mask.SetActive(true); Glow.SetActive(true); ModAPI.CreateParticleEffect("Spark", Glow.transform.position); SoundSource.Play();
        }
        else
        {
            Mask.SetActive(false); Glow.SetActive(false); ModAPI.CreateParticleEffect("Spark", Glow.transform.position); SoundSource.Play();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }
    private void EvaluateCollision(Collision2D collision)
    {
        if (Activated)
        {
            int contacts = collision.GetContacts(StunstickBehaviour.buffer);
            if (Utils.GetMinImpulse(StunstickBehaviour.buffer, contacts) < ForceNeeded)
            {
                return;
            }
            ModAPI.CreateParticleEffect("Spark", collision.contacts[0].point);
            ApplyCharge(collision.transform.GetComponent<PhysicalBehaviour>());
        }
    }
    void ApplyCharge(PhysicalBehaviour physBehaviour)
    {
        physBehaviour.Charge = Mathf.Max(physBehaviour.Charge, ChargeAmount);
    }
}