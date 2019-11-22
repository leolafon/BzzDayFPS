using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public float impactForce = 30f;
    public Camera cam;
    public ParticleSystem flash;
    public GameObject impact;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        flash.Play();
        audioSource.Play();

        RaycastHit hit;
        bool doesHit = Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range);
        if (doesHit == true)
        {
            Debug.Log(hit.transform.name);
            GameObject impactInstance = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactInstance, 1f);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }
        }
    }
}
