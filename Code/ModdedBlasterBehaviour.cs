using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UmmAPI;

public class ModdedBlasterBehaviour : MonoBehaviour
{
    public int BoltsPerShot = 1;
    public float InaccuracyMultiplier = 0.05f;
    public float Recoil = 15f;
    public float ScreenShakeMultiplier = 2f;
    public Vector2 barrelPosition;
    public Vector2 barrelDirection = new Vector2(1, 0);

    public float Interval = 0.1f;
    public bool Automatic = false;

    public AudioClip[] Clips;

    public GameObject Bolt;
    public float BoltDamage = 150f;
    public float BoltSpeed = 100f;
    public float BoltThickness = 0.1f;
    public float BoltTrailLifetime = 0.025f;
    public bool BoltGlows = true;
    public Color BlasterColor;
    public Color BlasterBoltColor;
    public float BoltAlpha;
    public Texture2D CustomBoltTexture;

    public GameObject MuzzleFlash;
    public bool HasMuzzleFlash = true;
    public float MuzzleFlashSize = 1f;

    private float t;
    private AudioSource AudioSource;

    public UnityEvent OnFire = new UnityEvent();
    public bool IgnoreUse = false;

    void Start()
    {
        if (GetComponent<DamagableMachineryBehaviour>())
        {
            GetComponent<DamagableMachineryBehaviour>().BehavioursToToggle = new MonoBehaviour[]
            {
                this,
            };
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        audioSource.dopplerLevel = 0f;
        audioSource.playOnAwake = false;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 75f;
        audioSource.spatialBlend = 1f;
        AudioSource = audioSource;

        Bolt = ModAPI.FindSpawnable("Blaster").Prefab.GetComponent<BlasterBehaviour>().Bolt;
        if (HasMuzzleFlash)
        {
            MuzzleFlash = UnityEngine.Object.Instantiate<GameObject>(ModAPI.FindSpawnable("Blaster").Prefab.GetComponent<BlasterBehaviour>().Muzzleflash.gameObject, Vector3.zero, Quaternion.identity);
            MuzzleFlash.transform.SetParent(transform);
            MuzzleFlash.transform.localPosition = barrelPosition;
            if (transform.lossyScale.x > 0)
                MuzzleFlash.transform.localRotation = Quaternion.identity;
            else
                MuzzleFlash.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            MuzzleFlash.transform.localScale = new Vector3(MuzzleFlashSize, MuzzleFlashSize, MuzzleFlashSize);

            ParticleSystem.MainModule main1 = MuzzleFlash.GetComponent<ParticleSystem>().main;
            main1.startColor = BlasterColor;
            ParticleSystem.MainModule main2 = MuzzleFlash.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            main2.startColor = BlasterColor;
        }
    }
    void Update()
    {
        t += Time.deltaTime;
    }
    public void Use(ActivationPropagation activation)
    {
        if (base.enabled && !IgnoreUse)
            Shoot();
    }
    public void UseContinuous(ActivationPropagation activation)
    {
        ContinuousActivationBehaviour.AssertState();
        if (!Automatic || t < Interval)
        {
            return;
        }
        if (base.enabled)
            Shoot();
    }

    void Shoot()
    {
        int r = UnityEngine.Random.Range(0, Clips.Length);
        AudioSource.PlayOneShot(Clips[r], 1f);
        for (int i = 0; i < BoltsPerShot; i++)
        {
            Vector2 vector = BarrelPosition;
            Vector2 a = BarrelDirection;
            t = 0f;
            GetComponent<PhysicalBehaviour>().rigidbody.AddForceAtPosition(a * -Recoil, vector, ForceMode2D.Impulse);
            Quaternion rotation;
            if (transform.lossyScale.x > 0)
                rotation = transform.rotation * Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-InaccuracyMultiplier * 100, InaccuracyMultiplier * 100));
            else
                rotation = transform.rotation * Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-InaccuracyMultiplier * 100, InaccuracyMultiplier * 100) + 180);
            GameObject bolt = UnityEngine.Object.Instantiate<GameObject>(Bolt, vector, rotation);
            if (HasMuzzleFlash || MuzzleFlash != null)
                MuzzleFlash.GetComponent<ParticleSystem>().Play();
            bolt.GetComponent<BlasterboltBehaviour>().Trail.colorGradient = new Gradient()
            {
                mode = GradientMode.Fixed,
                alphaKeys = new GradientAlphaKey[1]
                {
                new GradientAlphaKey(BoltAlpha, 0f)
                },
                colorKeys = new GradientColorKey[1]
                {
                new GradientColorKey(BlasterBoltColor, 0f)
                }
            };

            GetComponent<Rigidbody2D>().AddForceAtPosition(a * -Recoil, vector, ForceMode2D.Impulse);
            CameraShakeBehaviour.main.Shake(0.5f * ScreenShakeMultiplier, vector, 1f);
            bolt.transform.GetChild(1).GetComponent<SpriteRenderer>().color = BlasterColor;
            bolt.transform.GetChild(2).GetComponent<SpriteRenderer>().color = BlasterColor;
            bolt.GetComponent<BlasterboltBehaviour>().damage = BoltDamage;
            bolt.GetComponent<BlasterboltBehaviour>().Speed = BoltSpeed;
            bolt.GetComponent<TrailRenderer>().startWidth = BoltThickness;
            bolt.GetComponent<TrailRenderer>().endWidth = BoltThickness;
            bolt.GetComponent<TrailRenderer>().time = BoltTrailLifetime;
            if (CustomBoltTexture)
            {
                Material CustomMaterial = Instantiate<Material>(ModAPI.FindMaterial("VeryBright"));
                CustomMaterial.mainTexture = CustomBoltTexture;
                CustomMaterial.color = BlasterBoltColor;
                bolt.GetComponent<TrailRenderer>().material = CustomMaterial;
                bolt.GetComponent<TrailRenderer>().numCapVertices = 0;
            }
            if (!BoltGlows)
            {
                Destroy(bolt.transform.GetChild(1).gameObject);
            }
            if (OnFire != null)
                OnFire.Invoke();
        }
    }
    public Vector2 BarrelPosition
    {
        get
        {
            return transform.TransformPoint(barrelPosition);
        }
    }
    public Vector2 BarrelDirection
    {
        get
        {
            return base.transform.TransformDirection(this.barrelDirection) * base.transform.localScale.x;
        }
    }
}