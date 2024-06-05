using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject PickedBolt;
    public bool Loaded;
    public float ShootForce = 6;
    private NoCollide Collider;

    protected Vector2 barrelDirection = new Vector2(1, 0);

    protected AudioSource SoundSource;

    public AudioClip ShotClip;
    public AudioClip LoadClip;

    void Start()
    {
        SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.dopplerLevel = 0f;
        SoundSource.playOnAwake = false;
        SoundSource.rolloffMode = AudioRolloffMode.Linear;
        SoundSource.minDistance = 1f;
        SoundSource.maxDistance = 25f;
        SoundSource.spatialBlend = 1f;
        SoundSource.volume = 1;
        SoundSource.clip = ShotClip;
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
    }
    public Vector2 GetBarrelDirection()
    {
        return base.transform.TransformDirection(barrelDirection) * base.transform.localScale.x;
    }
    void Use(ActivationPropagation activationPropagation)
    {
        if (!Loaded)
            return;
        else
        {
            Destroy(PickedBolt.GetComponent<FixedJoint2D>());
            PickedBolt.layer = 9;
            PickedBolt.transform.parent = null;
            PickedBolt.GetComponent<Rigidbody2D>().AddForce(GetBarrelDirection() * ShootForce, ForceMode2D.Impulse);
            PickedBolt = null;
            Loaded = false;

            SoundSource.PlayOneShot(ShotClip);
        }
    }

    void Update()
    {
        if (PickedBolt != null)
        {
            PickedBolt.GetComponent<PhysicalBehaviour>().Temperature += 1f * Time.timeScale;
            //PickedBolt.transform.localPosition = new Vector3(0.3142857f, 0.04285714f);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Crossbow Bolt" + " [HLGM]" && !Loaded)
        {
            Loaded = true;
            PickedBolt = collision.gameObject;

            if (Collider == null) Collider = gameObject.AddComponent<NoCollide>();
            Collider.NoCollideSetA = PickedBolt.GetComponents<Collider2D>();
            Collider.NoCollideSetB = GetComponents<Collider2D>();

            PickedBolt.layer = 10;
            PickedBolt.GetComponent<PhysicalBehaviour>().IsWeightless = true;
            PickedBolt.transform.SetParent(transform);
            SoundSource.PlayOneShot(LoadClip);

            StartCoroutine(SetupBolt(0f));
        }
    }
    IEnumerator SetupBolt(float time)
    {
        new WaitForSeconds(0.5f);
        PickedBolt.transform.localPosition = new Vector3(0.3142857f, 0.04285714f);
        PickedBolt.transform.localRotation = Quaternion.identity;
        PickedBolt.AddComponent<FixedJoint2D>().connectedBody = gameObject.GetComponent<Rigidbody2D>();
        yield return null;
    }
}
