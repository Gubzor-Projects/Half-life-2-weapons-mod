using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UmmAPI;
namespace Mod
{
    public class Mod : MonoBehaviour
    {
        public static string ModTag = " [HLGM]";
        public static Sprite categorySprite = ModAPI.LoadSprite("Category.png");
        public static void Main()
        {
            ModAPI.RegisterCategory("Half Life 2 Weapons Mod", "Half Life 2 Weapons mod. Contains alt fire along-side ammo system", categorySprite);
            #region Content
            #region Crowbar
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Crowbar" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "Gordon's main and first weapon.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Crowbar.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Crowbar.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(0f, 0f),
                        };
                        physicalBehaviour.Properties = ModAPI.FindPhysicalProperties("Metal");
                        physicalBehaviour.Properties.SoftImpact = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/crowbar_impact_1.wav"),
                            ModAPI.LoadSound("SFX/crowbar_impact_2.wav"),
                        };
                        physicalBehaviour.Properties.HardImpact = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/crowbar_impact_1.wav"),
                            ModAPI.LoadSound("SFX/crowbar_impact_2.wav"),
                        };
                        UmAPI.SetPhysicalMass(physicalBehaviour, 12f);
                    }
                }
            );
            #endregion
            #region Stunstick
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Stun Baton" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Stun Baton, or Stunstick, is an electrified baton used by Civil Protection officers to enforce the law on unruly citizens with an iron fist.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Stunstick.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Stunstick.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(0f, -0.3f),
                        };
                        UmAPI.SetPhysicalMass(physicalBehaviour, 5f);
                        StunstickBehaviour stunstickBehaviour = Instance.GetOrAddComponent<StunstickBehaviour>();
                        stunstickBehaviour.Clip = ModAPI.LoadSound("SFX/spark1.wav");
                        stunstickBehaviour.Mask = UmAPI.CreateMaskObject(Instance, ModAPI.LoadSprite("Sprites/Stunstick Mask.png"), new Color(1, 0.5f, 0, 1), 0.03f, new Color(1, 0.5f, 0, 1), 1, 1, new Vector3(0, 0.324f), false);
                    }
                }
            );
            #endregion
            #region USP Match
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "USP Match" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The USP Match, also known as the 9mm Pistol, is a semi-automatic handgun and the first firearm Player acquires. Has 18 round magazine and has no special abilities.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/USP Match.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        ModAPI.KeepExtraObjects();
                        Instance.transform.Find("Slide").GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/USP Match Slide.png");
                        UmAPI.OffsetSlide(Instance, Instance.transform.GetChild(0), -1.5f, 0.5f);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/USP Match.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.14295f, -0.0333f)
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/pistol_fire2.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.2157f, 0.0707f);

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/9mm casing.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 5 * ModAPI.PixelSize;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 18;
                        ammoBehaviour.MaxAdditionalAmmo = 150;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/pistol_reload1.wav");
                    }
                }
            );
            #endregion
            #region Alyxs Gun
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "Alyx's Gun" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "Alyx Vance's trademark weapon, most of the times referred to as \"Alyx's Gun\". High firerate.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Alyxs Gun.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        ModAPI.KeepExtraObjects();
                        Instance.transform.Find("Slide").GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Alyx Gun Slide.png");
                        UmAPI.SetSlideLength(Instance.transform.GetChild(0), 3f);
                        UmAPI.OffsetSlide(Instance, Instance.transform.GetChild(0), -5.5f, 1f);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Alyx Gun.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.1609f, -0.0091f)
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/ar1_dist1.wav"),
                            ModAPI.LoadSound("SFX/ar1_dist2.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.2322f, 0.0988f);
                        firearmBehaviour.Automatic = true;
                        firearmBehaviour.AutomaticFireInterval = 0.09f;

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/9mm casing.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 5 * ModAPI.PixelSize;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 30;
                        ammoBehaviour.MaxAdditionalAmmo = 150;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/pistol_reload1.wav");
                    }
                }
            );
            #endregion
            #region Revolver
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "Colt Python (.357 Magnum Revolver)" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Colt Python (or .357 Magnum Revolver) is a powerful handgun, which is more accurate and powerful than the USP Match.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Revolver.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Revolver.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.1905f, -0.0423f)
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/357_fire2.wav"),
                            ModAPI.LoadSound("SFX/357_fire3.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.2157f, 0.0707f);
                        firearmBehaviour.EjectShells = false;

                        Cartridge customCartrige = ModAPI.FindCartridge("9mm");
                        customCartrige.Damage = 40f;

                        firearmBehaviour.Cartridge = customCartrige;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 6;
                        ammoBehaviour.MaxAdditionalAmmo = 12;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/357_spin1.wav");
                    }
                }
            );
            #endregion
            #region SMG
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "SMG" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Heckler & Koch MP7, also known as the SMG1, is a compact, fully automatic firearm.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/SMG1.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/SMG1.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.2453f, -0.0842f),
                            new Vector3(0.1596f, -0.0842f),
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/smg1_fire1.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.379f, 0.0707f);
                        firearmBehaviour.Automatic = true;
                        firearmBehaviour.InitialInaccuracy = 0.06f;
                        firearmBehaviour.AutomaticFireInterval = 0.075f;

                        Cartridge customCartrige = ModAPI.FindCartridge("9mm");
                        customCartrige.Damage = 4f;

                        firearmBehaviour.Cartridge = customCartrige;

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/9mm casing.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 5 * ModAPI.PixelSize;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 45;
                        ammoBehaviour.MaxAdditionalAmmo = 225;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/smg1_reload.wav");

                        Sprite GrenadeSprite = ModAPI.LoadSprite("Sprites/SMG1 Grenade.png");

                        AudioSource SoundSource;
                        SoundSource = Instance.AddComponent<AudioSource>();
                        SoundSource.dopplerLevel = 0f;
                        SoundSource.playOnAwake = false;
                        SoundSource.rolloffMode = AudioRolloffMode.Linear;
                        SoundSource.minDistance = 1f;
                        SoundSource.maxDistance = 25f;
                        SoundSource.spatialBlend = 1f;
                        SoundSource.volume = 1;
                        SoundSource.clip = ModAPI.LoadSound("SFX/grenade_launcher1.wav");

                        AltFireBehaviour AltFire = Instance.GetOrAddComponent<AltFireBehaviour>();
                        AltFire.OnActivation = new UnityEvent();
                        AltFire.OnActivation.AddListener(() =>
                        {
                            #region Grenade
                            Vector2 barrelDirection = new Vector2(1, 0);
                            float ShootForce = 0.5f;
                            Vector2 GetBarrelDirection()
                            {
                                return Instance.transform.TransformDirection(barrelDirection) * Instance.transform.localScale.x;
                            }
                            SoundSource.Play();
                            GameObject Grenade = Instantiate<GameObject>(ModAPI.FindSpawnable("Handgrenade").Prefab, Instance.transform.position, Instance.transform.rotation);
                            Grenade.transform.SetParent(Instance.transform);
                            Grenade.transform.localPosition = new Vector2(0.419f, 0.0707f);
                            Grenade.transform.localScale = Vector3.one;
                            Grenade.transform.localRotation = Quaternion.identity;
                            Grenade.transform.SetParent(null);
                            Grenade.GetComponent<SpriteRenderer>().sprite = GrenadeSprite;
                            Grenade.GetComponent<Rigidbody2D>().AddForce(GetBarrelDirection() * ShootForce, ForceMode2D.Impulse);
                            Grenade.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
                            Grenade.FixColliders();
                            Grenade.GetComponent<ExplosiveBehaviour>().ImpactForceThreshold = 0;
                            Grenade.GetComponent<ExplosiveBehaviour>().Delay = 0;
                            Instance.transform.Find("MuzzleSmoke(Clone)").GetComponent<ParticleSystem>().Play();
                            Destroy(Grenade.transform.Find("HandgrenadePin").gameObject);
                            Destroy(Grenade.transform.Find("HandgrenadeLever").gameObject);
                            #endregion
                        });
                    }
                }
            );
            #endregion
            #region Overwatch Standard Issue Pulse Rifle
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Overwatch Standard Issue Pulse Rifle (AR2)" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Overwatch Standard Issue Pulse Rifle, also known as Pulse Rifle, is a Dark Energy/pulse-powered assault rifle.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/AR2.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/AR2.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.1449f, -0.1007f),
                            new Vector3(0.1168f, -0.1015f),
                        };

                        ModdedBlasterBehaviour firearmBehaviour = Instance.GetOrAddComponent<ModdedBlasterBehaviour>();
                        firearmBehaviour.barrelPosition = new Vector3(0.5964f, 0.0572f);
                        firearmBehaviour.Interval = 0.1f;
                        firearmBehaviour.Automatic = true;
                        firearmBehaviour.BlasterColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BlasterBoltColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BoltAlpha = 1f;
                        firearmBehaviour.CustomBoltTexture = ModAPI.LoadTexture("Sprites/HDparticle.png");
                        firearmBehaviour.BoltDamage = 8f;
                        firearmBehaviour.BoltSpeed = 95;
                        firearmBehaviour.BoltThickness = 0.1f;
                        firearmBehaviour.BoltTrailLifetime = 0.02f;
                        firearmBehaviour.BoltGlows = true;
                        firearmBehaviour.InaccuracyMultiplier = 0.04f;
                        firearmBehaviour.MuzzleFlashSize = 0.6f;
                        firearmBehaviour.Recoil = 0.1f;
                        firearmBehaviour.Clips = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/ar1.wav"),
                            ModAPI.LoadSound("SFX/ar2.wav"),
                        };

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 30;
                        ammoBehaviour.MaxAdditionalAmmo = 60;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/smg1_reload.wav");
                    }
                }
            );
            #endregion
            #region OICW
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "OICW" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "OICW, is a weapon cut from Half-Life 2. It can be found in the playable Half-Life 2 Beta.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/OICW.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/OICW.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.202f, -0.091f),
                            new Vector3(0.256f, -0.034f),
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/OICW_1.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.661f, 0.002f);
                        firearmBehaviour.Automatic = true;
                        firearmBehaviour.InitialInaccuracy = 0.02f;
                        firearmBehaviour.AutomaticFireInterval = 0.1f;

                        Cartridge customCartrige = ModAPI.FindCartridge("9mm");
                        customCartrige.Damage = 8f;

                        firearmBehaviour.Cartridge = customCartrige;

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/5.56mm casing.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 5 * ModAPI.PixelSize;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 30;
                        ammoBehaviour.MaxAdditionalAmmo = 150;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/OICW_Reload.wav");

                        Sprite GrenadeSprite = ModAPI.LoadSprite("Sprites/SMG1 Grenade.png");

                        AudioSource SoundSource;
                        SoundSource = Instance.AddComponent<AudioSource>();
                        SoundSource.dopplerLevel = 0f;
                        SoundSource.playOnAwake = false;
                        SoundSource.rolloffMode = AudioRolloffMode.Linear;
                        SoundSource.minDistance = 1f;
                        SoundSource.maxDistance = 25f;
                        SoundSource.spatialBlend = 1f;
                        SoundSource.volume = 1;
                        SoundSource.clip = ModAPI.LoadSound("SFX/OICW_Alt.wav");

                        AltFireBehaviour AltFire = Instance.GetOrAddComponent<AltFireBehaviour>();
                        AltFire.OnActivation = new UnityEvent();
                        AltFire.OnActivation.AddListener(() =>
                        {
                            #region Grenade
                            Vector2 barrelDirection = new Vector2(1, 0);
                            float ShootForce = 0.5f;
                            Vector2 GetBarrelDirection()
                            {
                                return Instance.transform.TransformDirection(barrelDirection) * Instance.transform.localScale.x;
                            }
                            SoundSource.Play();
                            GameObject Grenade = Instantiate<GameObject>(ModAPI.FindSpawnable("Handgrenade").Prefab, Instance.transform.position, Instance.transform.rotation);
                            Grenade.transform.SetParent(Instance.transform);
                            Grenade.transform.localPosition = new Vector2(0.661f, 0.002f);
                            Grenade.transform.localScale = Vector3.one;
                            Grenade.transform.localRotation = Quaternion.identity;
                            Grenade.transform.SetParent(null);
                            Grenade.GetComponent<SpriteRenderer>().sprite = GrenadeSprite;
                            Grenade.GetComponent<Rigidbody2D>().AddForce(GetBarrelDirection() * ShootForce, ForceMode2D.Impulse);
                            Grenade.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
                            Grenade.FixColliders();
                            Grenade.GetComponent<ExplosiveBehaviour>().ImpactForceThreshold = 0;
                            Grenade.GetComponent<ExplosiveBehaviour>().Delay = 0;
                            Instance.transform.Find("MuzzleSmoke(Clone)").GetComponent<ParticleSystem>().Play();
                            Destroy(Grenade.transform.Find("HandgrenadePin").gameObject);
                            Destroy(Grenade.transform.Find("HandgrenadeLever").gameObject);
                            #endregion
                        });
                    }
                }
            );
            #endregion
            #region Spas 12
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "Spas 12" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "SPAS-12 is a powerful pump-action shotgun that fires buckshot in a cone-shaped pattern.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Spas 12.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        ModAPI.KeepExtraObjects();
                        Instance.transform.Find("Slide").GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Spas 12 Slide.png");
                        UmAPI.OffsetSlide(Instance, Instance.transform.GetChild(0), 3f, -2f);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Spas 12.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.4945f, -0.054f),
                            new Vector3(0.1011f, -0.025f),
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/shotgun_fire6.wav"),
                            ModAPI.LoadSound("SFX/shotgun_fire7.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.5602f, 0.0854f);
                        firearmBehaviour.BulletsPerShot = 7;
                        firearmBehaviour.InitialInaccuracy = 0.08f;

                        Cartridge customCartrige = ModAPI.FindCartridge("9mm");
                        customCartrige.Damage = 8f;
                        customCartrige.Recoil = 0.2f;

                        firearmBehaviour.Cartridge = customCartrige;

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/Buckshot.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 7 * ModAPI.PixelSize;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 6;
                        ammoBehaviour.MaxAdditionalAmmo = 30;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/shotgun_reload1.wav");

                        AltFireBehaviour AltFire = Instance.GetOrAddComponent<AltFireBehaviour>();
                        AltFire.OnActivation = new UnityEvent();
                        AltFire.OnActivation.AddListener(() =>
                        {
                            if (ammoBehaviour.CurrentAmmo > 1)
                                for (int i = 0; i < 2; i++)
                                    firearmBehaviour.Shoot();
                            else
                                firearmBehaviour.Shoot();
                        });
                    }
                }
            );
            #endregion
            #region Annabelle
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "Annabelle" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Annabelle is the rifle used by Father Grigori in Ravenholm.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Annabelle.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Annabelle.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.337f, 0.008f),
                            new Vector3(0.168f, 0.055f),
                        };

                        FirearmBehaviour firearmBehaviour = Instance.GetComponent<FirearmBehaviour>();
                        firearmBehaviour.ShotSounds = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/shotgun_fire6.wav"),
                            ModAPI.LoadSound("SFX/shotgun_fire7.wav"),
                        };
                        firearmBehaviour.barrelPosition = new Vector2(0.7894f, 0.1149f);
                        firearmBehaviour.EjectShells = false;

                        Cartridge customCartrige = ModAPI.FindCartridge("9mm");
                        customCartrige.Damage = 40f;
                        customCartrige.Recoil = 0.5f;

                        firearmBehaviour.Cartridge = customCartrige;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 2;
                        ammoBehaviour.MaxAdditionalAmmo = 12;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/shotgun_reload1.wav");

                        Material casingMaterial = Resources.Load<GameObject>("Prefabs/BulletCasing").GetComponent<ParticleSystemRenderer>().sharedMaterial;
                        Texture2D casingSprite = ModAPI.LoadTexture("Sprites/Buckshot.png");
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial = Instantiate<Material>(casingMaterial);
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystemRenderer>().sharedMaterial.mainTexture = casingSprite;
                        Instance.transform.Find("BulletCasing(Clone)").GetComponent<ParticleSystem>().startSize = 7 * ModAPI.PixelSize;
                    }
                }
            );
            #endregion
            #region Crossbow
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Resistance Crossbow" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Resistance Crossbow is a long-range sniper weapon featured in Half-Life 2 and Episodes. It is ruthlessly effective against distant, unsuspecting opponents, but is difficult to utilize in melee combat or against fast moving opponents due to its very slow reload and the low velocity of the projectile.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Crossbow.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Crossbow.png");
                        Instance.FixColliders();
                        Crossbow Crossbow = Instance.GetOrAddComponent<Crossbow>();
                        Crossbow.ShotClip = ModAPI.LoadSound("SFX/fire1.wav");
                        Crossbow.LoadClip = ModAPI.LoadSound("SFX/bolt_load1.wav");
                    }
                }
            );
            #endregion
            #region Crossbow Bolt
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Crossbow Bolt" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Crossbow Bolt.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Crossbow Bolt.png");
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        Instance.FixColliders();
                        var properties = ModAPI.FindPhysicalProperties("Metal");
                        properties.Sharp = true;
                        properties.IgnoreStabResistance = true;
                        properties.SharpAxes = new SharpAxis[]
                        {
                            new SharpAxis(Vector2.right, -0.4f, 0.5f, true, false),
                            new SharpAxis(Vector2.left, -0.4f, 0.5f, true, false)
                        };
                        Instance.GetComponent<PhysicalBehaviour>().Properties = properties;
                        Instance.GetComponent<GlowingHotMetalBehaviour>().UpperTemperatureBound = 100;
                    }
                }
            );
            #endregion
            #region Grenade
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Handgrenade"),
                    NameOverride = "MK3A2 Grenade" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The MK3A2 Grenade is a Combine weapon featured in Half-Life 2 and its episodes. It is referred to as an Extractor or Bouncer by Combine Soldiers.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/MK3A2.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/MK3A2.png");
                        Instance.FixColliders();

                        GameObject Lever = Instance.transform.Find("HandgrenadeLever").gameObject;
                        GameObject Pin = Instance.transform.Find("HandgrenadePin").gameObject;

                        Lever.SetActive(false);
                        Pin.transform.localPosition = new Vector3(0f, 0.0714f);

                        ExplosiveBehaviour explosive = Instance.GetComponent<ExplosiveBehaviour>();
                        explosive.Delay = 2.3f;

                        GrenadeFuse grenadeFuse = Instance.AddComponent<GrenadeFuse>();
                        Gradient gradient = new Gradient();
                        GradientAlphaKey alphaKey1 = new GradientAlphaKey();
                        alphaKey1.alpha = 1f;
                        alphaKey1.time = 0f;
                        GradientAlphaKey alphaKey2 = new GradientAlphaKey();
                        alphaKey2.alpha = 0.2f;
                        alphaKey2.time = 0.2f;
                        GradientAlphaKey alphaKey3 = new GradientAlphaKey();
                        alphaKey3.alpha = 0f;
                        alphaKey3.time = 0.8f;
                        gradient.alphaKeys = new GradientAlphaKey[]
                        {
                            alphaKey1,
                            alphaKey2,
                            alphaKey3,
                        };
                        GradientColorKey colorKey1 = new GradientColorKey();
                        colorKey1.color = new Color(1f, 0f, 0f, 0.3f);
                        colorKey1.time = 0f;
                        GradientColorKey colorKey2 = new GradientColorKey();
                        colorKey2.color = new Color(1f, 0f, 0f, 0.3f);
                        colorKey2.time = 0.2f;
                        GradientColorKey colorKey3 = new GradientColorKey();
                        colorKey3.color = new Color(1f, 0f, 0f, 0.3f);
                        colorKey3.time = 0.8f;
                        gradient.colorKeys = new GradientColorKey[]
                        {
                            colorKey1,
                            colorKey2,
                            colorKey3,
                        };
                        grenadeFuse.Gradient = gradient;
                        grenadeFuse.Tick = ModAPI.LoadSound("SFX/tick1.wav");

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(0f, 0f),
                        };
                        UmAPI.SetPhysicalMass(physicalBehaviour, 1f);
                    }
                }
            );
            #endregion
            #region RPG
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Resistance RPG" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "RPG, is a powerful weapon that is sometimes used by Rebels against Combine forces.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/RPG.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/RPG.png");
                        Instance.FixColliders();

                        CustomRPG RPGBehaviour = Instance.GetOrAddComponent<CustomRPG>();
                        RPGBehaviour.ProjectileSprite = ModAPI.LoadSprite("Sprites/RPG Missile.png");
                        RPGBehaviour.barrelPosition = new Vector2(0.847f, 0.109f);
                        AudioSource HighSource = Instance.AddComponent<AudioSource>();
                        Instance.AddComponent<AudioSourceTimeScaleBehaviour>();
                        HighSource.dopplerLevel = 0f;
                        HighSource.playOnAwake = false;
                        HighSource.rolloffMode = AudioRolloffMode.Linear;
                        HighSource.minDistance = 1f;
                        HighSource.maxDistance = 75f;
                        HighSource.spatialBlend = 1f;
                        HighSource.clip = ModAPI.LoadSound("SFX/rocketfire1.wav");
                        RPGBehaviour.AudioSource = HighSource;
                        RPGBehaviour.LauncherType = "Bazooka";
                        GameObject Tip = new GameObject();
                        Tip.transform.SetParent(Instance.transform);
                        Tip.transform.localPosition = new Vector3(0.643f, -0.056f);
                        Tip.transform.localScale = Vector3.one;
                        RPGBehaviour.LinePoint = Tip.transform;
                        RPGBehaviour.PosSpeed = 45f;
                        RPGBehaviour.RotationSpeed = 60f;

                        LineRenderer lineRenderer = Instance.AddComponent<LineRenderer>();
                        lineRenderer.material = ModAPI.FindMaterial("VeryBright");
                        lineRenderer.material.mainTexture = ModAPI.LoadTexture("Sprites/RPG Line.png");
                        lineRenderer.startColor = new Color(1f, 0.2f, 0.2f, 0.05f);
                        lineRenderer.endColor = new Color(1f, 0.2f, 0.2f, 0.05f);
                        lineRenderer.widthMultiplier = 0.03f;
                        lineRenderer.sortingOrder = -1;

                        RPGBehaviour.Line = lineRenderer;

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 1;
                        ammoBehaviour.MaxAdditionalAmmo = 2;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/smg1_reload.wav");
                    }
                }
            );
            #endregion
            #region AR3
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Emplacement Gun (AR3)" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Emplacement Gun, also known as the Mounted Gun, is a mounted pulse gun manufactured by the Combine. It is similar to the Overwatch Standard Issue Pulse Rifle, except that it cannot be removed from its mount, shoots at a faster rate and does not fire Energy Balls.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Emplacement Gun.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Emplacement Gun.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.848f, -0.028f)
                        };

                        ModdedBlasterBehaviour firearmBehaviour = Instance.GetOrAddComponent<ModdedBlasterBehaviour>();
                        firearmBehaviour.barrelPosition = new Vector3(0.877f, 0.042f);
                        firearmBehaviour.Interval = 0.06f;
                        firearmBehaviour.Automatic = true;
                        firearmBehaviour.BlasterColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BlasterBoltColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BoltAlpha = 1f;
                        firearmBehaviour.CustomBoltTexture = ModAPI.LoadTexture("Sprites/HDparticle.png");
                        firearmBehaviour.BoltDamage = 15f;
                        firearmBehaviour.BoltSpeed = 70;
                        firearmBehaviour.BoltThickness = 0.4f;
                        firearmBehaviour.BoltTrailLifetime = 0.05f;
                        firearmBehaviour.BoltGlows = true;
                        firearmBehaviour.InaccuracyMultiplier = 0.02f;
                        firearmBehaviour.MuzzleFlashSize = 1.1f;
                        firearmBehaviour.Recoil = 0.4f;
                        firearmBehaviour.Clips = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/ar1_dist1.wav"),
                            ModAPI.LoadSound("SFX/ar1_dist2.wav"),
                        };

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 3000000;
                        ammoBehaviour.MaxAdditionalAmmo = 60000000;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/smg1_reload.wav");
                    }
                }
            );
            #endregion
            #region Overwatch Sniper Rifle
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Rod"),
                    NameOverride = "Overwatch Sniper Rifle" + ModTag,
                    NameToOrderByOverride = "Z",
                    DescriptionOverride = "The Overwatch Sniper Rifle is a pulse rifle identifiable by its signature blue laser sight, Overwatch Snipers use them to guard key areas.",
                    CategoryOverride = ModAPI.FindCategory("Half Life 2 Weapons Mod"),
                    ThumbnailOverride = ModAPI.LoadSprite("Thumbnails/Combine Sniper Rifle.png", 5f),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Sprites/Combine Sniper Rifle.png");
                        Instance.FixColliders();

                        PhysicalBehaviour physicalBehaviour = Instance.GetComponent<PhysicalBehaviour>();
                        physicalBehaviour.HoldingPositions = new Vector3[]
                        {
                            new Vector3(-0.558f, -0.152f),
                            new Vector3(-0.15f, -0.122f),
                        };

                        ModdedBlasterBehaviour firearmBehaviour = Instance.GetOrAddComponent<ModdedBlasterBehaviour>();
                        firearmBehaviour.barrelPosition = new Vector3(0.966f, 0.07f);
                        firearmBehaviour.Automatic = false;
                        firearmBehaviour.BlasterColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BlasterBoltColor = new Color(0.3254717f, 0.9727463f, 1f, 1f);
                        firearmBehaviour.BoltAlpha = 1f;
                        firearmBehaviour.CustomBoltTexture = ModAPI.LoadTexture("Sprites/HDparticle.png");
                        firearmBehaviour.BoltDamage = 175f;
                        firearmBehaviour.BoltSpeed = 200;
                        firearmBehaviour.BoltThickness = 0.5f;
                        firearmBehaviour.BoltTrailLifetime = 0.03f;
                        firearmBehaviour.BoltGlows = true;
                        firearmBehaviour.InaccuracyMultiplier = 0f;
                        firearmBehaviour.MuzzleFlashSize = 2f;
                        firearmBehaviour.Recoil = 2f;
                        firearmBehaviour.Clips = new AudioClip[]
                        {
                            ModAPI.LoadSound("SFX/ar1_dist1.wav"),
                            ModAPI.LoadSound("SFX/ar1_dist2.wav"),
                        };

                        AmmoBehaviour ammoBehaviour = Instance.GetOrAddComponent<AmmoBehaviour>();
                        ammoBehaviour.Ammo = 1;
                        ammoBehaviour.MaxAdditionalAmmo = 1;
                        ammoBehaviour.ReloadClip = ModAPI.LoadSound("SFX/smg1_reload.wav");
                    }
                }
            );
            #endregion
            #endregion
        }
    }
}
public class FirstSpawn : MonoBehaviour
{

}