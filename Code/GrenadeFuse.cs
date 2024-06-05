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

public class GrenadeFuse : MonoBehaviour
{
    public float Delay = 4;
    public bool used = false;
    public bool fused = false;
    public LightSprite Renderer;
    public Gradient Gradient;
    public float Duration;
    protected float num;
    protected AudioSource AudioSource;
    public AudioClip Tick;
    void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        audioSource.dopplerLevel = 0f;
        audioSource.playOnAwake = false;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 75f;
        audioSource.spatialBlend = 1f;
        audioSource.clip = Tick;
        AudioSource = audioSource;
    }
    void Update()
    {
        if (fused)
        {
            if(num == 0)
                AudioSource.Play();
            num += Time.deltaTime;
            Renderer.Color = Gradient.Evaluate(num);
            if (num >= 1f / Duration)
            {
                num = 0f;
            }
        }
    }
    void Use(ActivationPropagation activationPropagation)
    {
        if (!used)
        {
            used = true;
            StartCoroutine(Fuse(Delay));
        }
    }
    private IEnumerator Fuse(float delay)
    {
        Duration = 1;
        fused = true;
        LightSprite Light = ModAPI.CreateLight(transform, Color.red, 1, 3f);
        Light.transform.localPosition = new Vector3(-0.0131f, 0.1577f);
        Renderer = Light;
        yield return new WaitForSeconds(1.5f);
        Duration = 4.5f;
        yield break;
    }
}