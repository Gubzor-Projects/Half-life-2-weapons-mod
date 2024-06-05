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
using UnityEngine.Animations;

public class CustomRPG : MonoBehaviour
{
    [SkipSerialisation]
    public string LauncherType = "Bazooka";
    public GameObject Projectile;
    public Sprite ProjectileSprite;
    [SkipSerialisation]
    public AudioSource AudioSource;
    public float ScreenShakeIntensity = 0.5f;
    public Vector2 barrelPosition;
    public Vector2 barrelDirection;
    public AudioClip[] LaunchingSounds;

    public Vector3 position;
    public Transform LinePoint;
    public LineRenderer Line;
    public float RotationSpeed = 5f;
    public float PosSpeed = 3f;

    public GameObject CurrentMissile;

    public UnityEvent OnFire = new UnityEvent();
    public bool IgnoreUse = false;

    void Start()
    {
        if (LauncherType == "Bazooka")
        {
            Projectile = ModAPI.FindSpawnable("Bazooka").Prefab.GetComponent<RocketLauncherBehaviour>().Projectile;
        }
        else if (LauncherType == "RPG")
        {
            Projectile = ModAPI.FindSpawnable("Rocket Launcher").Prefab.GetComponent<RocketLauncherBehaviour>().Projectile;
        }
    }

    void Update()
    {
        float axis = 0;
        if (transform.lossyScale.x > 0)
            axis = 1;
        else
            axis = -1;
        RaycastHit2D hit = Physics2D.Raycast(LinePoint.position, LinePoint.right * axis);

        Line.SetPosition(0, LinePoint.position);
        Line.SetPosition(1, hit.point);

        if (hit.collider != null && !hit.transform.GetComponent<CustomRPG>())
        {
            position = hit.point;
        }

        if (CurrentMissile != null)
        {
            Vector2 direction = (position - CurrentMissile.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(CurrentMissile.transform.forward, direction);
            CurrentMissile.transform.rotation = Quaternion.Lerp(CurrentMissile.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            CurrentMissile.transform.position = Vector2.MoveTowards(CurrentMissile.transform.position, position, PosSpeed * Time.deltaTime);
        }
    }
    public void Use(ActivationPropagation activation)
    {
        if (!IgnoreUse && CurrentMissile == null)
        {
            float axis = 0;
            if (transform.lossyScale.x > 0)
                axis = 1;
            else
                axis = -1;
            if (!base.enabled)
            {
                return;
            }
            if (this.LaunchingSounds != null && this.LaunchingSounds.Length != 0)
            {
                this.AudioSource.PlayOneShot(this.LaunchingSounds.PickRandom<AudioClip>());
            }
            else
            {
                this.AudioSource.PlayOneShot(this.AudioSource.clip);
            }
            CameraShakeBehaviour.main.Shake(ScreenShakeIntensity, transform.position, 1f);
            GameObject Projectile = UnityEngine.Object.Instantiate<GameObject>(this.Projectile, transform.position, Quaternion.identity);
            Projectile.transform.SetParent(transform);
            Projectile.transform.localPosition = barrelPosition;
            Projectile.transform.localRotation = Quaternion.identity;
            Projectile.transform.localScale = Vector3.one;
            Projectile.transform.SetParent(null);
            Projectile.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ProjectileSprite;
            Projectile.GetComponent<LaunchedRocketBehaviour>().MaxSpeed = 0f;
            Projectile.GetComponent<LaunchedRocketBehaviour>().WobbleIntensity *= 0;
            Projectile.transform.GetChild(1).localPosition = new Vector3(-0.1491f * axis, 0.3885f);
            Projectile.transform.GetChild(1).localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            BoxCollider2D coll = Projectile.AddComponent<BoxCollider2D>();
            coll.size = new Vector2(0.2f, 0.2f);
            coll.isTrigger = true;
            Projectile.AddComponent<CustomRocketLogic>();

            CurrentMissile = Projectile;


            if (OnFire != null)
                OnFire.Invoke();
        }
    }

    void OnDestroy()
    {
        if (CurrentMissile != null)
        {
            Destroy(CurrentMissile);
        }
    }
}