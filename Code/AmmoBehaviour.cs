using System;
using UnityEngine;
using MPW;

public class AmmoBehaviour : MonoBehaviour
{
    public int Ammo = 6;
    protected float ReloadTime;
    public int CurrentAmmo;
    protected bool FirstSpawn;
    public AudioSource ReloadSource;
    protected bool reloading;
    public AudioClip ReloadClip;

    public bool ChargeAmmo = true;

    public int MaxAdditionalAmmo = 30;
    public int AdditionalAmmo = 0;

    void Start()
    {
        ReloadSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        ReloadSource.dopplerLevel = 0f;
        ReloadSource.playOnAwake = false;
        ReloadSource.rolloffMode = AudioRolloffMode.Linear;
        ReloadSource.minDistance = 1f;
        ReloadSource.maxDistance = 15f;
        ReloadSource.spatialBlend = 1f;
        ReloadSource.clip = ReloadClip;
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        if (!FirstSpawn)
        {
            CurrentAmmo = Ammo;
            ReloadTime = ReloadClip.length;
            FirstSpawn = true;
        }
        if (GetComponent<FirearmBehaviour>())
        {
            GetComponent<FirearmBehaviour>().OnFire.AddListener(() =>
            {
                UpdateAmmo();
            });
        }
        if (GetComponent<ModdedBlasterBehaviour>())
        {
            GetComponent<ModdedBlasterBehaviour>().OnFire.AddListener(() =>
            {
                UpdateAmmo();
            });
        }
        if (GetComponent<CustomRPG>())
        {
            GetComponent<CustomRPG>().OnFire.AddListener(() =>
            {
                UpdateAmmo();
            });
        }
    }

    public void UpdateAmmo()
    {
        if (!reloading)
        {
            if (CurrentAmmo > 1)
            {
                CurrentAmmo -= 1;
                Debug.Log(CurrentAmmo);
            }
            else
            {
                ReloadRutine();
            }
        }
        else return;
    }
    void ReloadRutine()
    {
        Debug.Log("Reloading Rutine");
        if (GetComponent<FirearmBehaviour>())
            GetComponent<FirearmBehaviour>().IgnoreUse = true;
        if (GetComponent<ModdedBlasterBehaviour>())
            GetComponent<ModdedBlasterBehaviour>().IgnoreUse = true;
        if (GetComponent<CustomRPG>())
            GetComponent<CustomRPG>().IgnoreUse = true;
            ReloadSource.Play();
            reloading = true;
            Invoke(nameof(Reload), ReloadTime);
    }
    void Reload()
    {
        Debug.Log("Reloaded");

        CurrentAmmo = Ammo;

        reloading = false;

        if(GetComponent<FirearmBehaviour>())
            GetComponent<FirearmBehaviour>().IgnoreUse = false;
        if (GetComponent<ModdedBlasterBehaviour>())
            GetComponent<ModdedBlasterBehaviour>().IgnoreUse = false;
        if (GetComponent<CustomRPG>())
            GetComponent<CustomRPG>().IgnoreUse = false;
    }
}