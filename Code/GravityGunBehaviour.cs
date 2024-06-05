using UnityEngine;

public class GravityGunBehaviour : MonoBehaviour
{
    public float PullDistance = 10f;
    public float PullForce = 10f;
    public float ShootForce = 900f;
    protected GameObject GrabbedObject;
    protected float GrabbedObjectOriginalMass;
    public Transform Tip;
    protected bool GrabbedSomething = false;

    protected AudioSource SoundSource;
    protected AudioSource SecondarySoundSource;

    public AudioClip S_TooHeavy;
    public AudioClip S_Launch;
    public AudioClip S_HoldLoop;
    public AudioClip S_ClawsOpen;
    public AudioClip S_ClawsClose;
    public AudioClip S_Drop;
    public AudioClip S_Pickup;

    void Start()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        SoundSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        SoundSource.dopplerLevel = 0f;
        SoundSource.playOnAwake = false;
        SoundSource.rolloffMode = AudioRolloffMode.Linear;
        SoundSource.minDistance = 1f;
        SoundSource.maxDistance = 25f;
        SoundSource.spatialBlend = 1f;
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();

        SecondarySoundSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
        SecondarySoundSource.dopplerLevel = 0f;
        SecondarySoundSource.playOnAwake = false;
        SecondarySoundSource.rolloffMode = AudioRolloffMode.Linear;
        SecondarySoundSource.minDistance = 1f;
        SecondarySoundSource.maxDistance = 25f;
        SecondarySoundSource.spatialBlend = 1f;
        gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
    }
    void Update()
    {

    }
    void Use(ActivationPropagation activationPropagation)
    {
        if (GrabbedSomething)
        {
            Destroy(GrabbedObject.GetComponent<FixedJoint2D>());
            GrabbedObject.GetComponent<Rigidbody2D>().AddForce(Tip.transform.right * ShootForce * GrabbedObject.GetComponent<Rigidbody2D>().mass);
            GrabbedSomething = false;

            ModAPI.CreateParticleEffect("Spark", Tip.position).transform.localScale = Vector3.one * 2f;

            SecondarySoundSource.clip = S_Launch;
            SecondarySoundSource.loop = false;
            SecondarySoundSource.Play();
            SoundSource.Stop();
        }
    }
    void SpecialActivation()
    {
        if(GrabbedSomething)
        {
            Destroy(GrabbedObject.GetComponent<FixedJoint2D>());
            GrabbedSomething = false;

            SecondarySoundSource.clip = S_Drop;
            SecondarySoundSource.loop = false;
            SecondarySoundSource.Play();
        }
    }
    void SpecialActivationContinuous(sbyte state)
    {
        if (!enabled) //this is not required to be added, but it will be useful in the future.
        {
            return;
        }

        if (state == 0)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            SecondarySoundSource.clip = S_ClawsOpen;
            SecondarySoundSource.loop = false;
            SecondarySoundSource.Play();
        }

        if (state == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(Tip.position, Tip.right, PullDistance);
            if (hit.collider != null && hit.collider.GetComponent<Rigidbody2D>() != null)
            {
                if (hit.collider.GetComponent<Rigidbody2D>().mass < 3f)
                {
                    PullObject(hit.transform.GetComponent<Rigidbody2D>());
                }
                else
                {
                    SecondarySoundSource.clip = S_TooHeavy;
                    SecondarySoundSource.loop = false;
                    SecondarySoundSource.Play();
                }
            }
        }

        if (state == 2)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            SecondarySoundSource.clip = S_ClawsClose;
            SecondarySoundSource.loop = false;
            SecondarySoundSource.Play();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.layer != 11 && !GrabbedSomething)
        {
            GrabbedSomething = true;
            GrabbedObject = other.gameObject;
            GrabbedObject.transform.SetParent(Tip.transform, false);
            GrabbedObject.AddComponent<FixedJoint2D>().connectedBody = transform.GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = GrabbedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                float xOffset = spriteRenderer.bounds.size.x / 2;
                GrabbedObject.transform.localPosition = new Vector3(xOffset, 0f, 0f);
            }

            GrabbedObject.transform.SetParent(null);

            SecondarySoundSource.clip = S_Pickup;
            SecondarySoundSource.loop = false;
            SecondarySoundSource.Play();

            SoundSource.clip = S_HoldLoop;
            SoundSource.loop = true;
            SoundSource.Play();
        }
    }

    public void PullObject(Rigidbody2D rigidbody)
    {
        if (rigidbody != null)
        {
            Vector2 direction = (transform.position - rigidbody.transform.position).normalized;
            float distance = Vector2.Distance(transform.position, rigidbody.transform.position);

            float pullForce = Mathf.Clamp(PullDistance - distance, 0f, PullDistance) * PullForce;

            rigidbody.AddForce(direction * pullForce);
        }
    }
}
