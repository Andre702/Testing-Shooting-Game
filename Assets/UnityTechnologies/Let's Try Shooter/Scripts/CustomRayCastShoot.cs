using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRayCastShoot : MonoBehaviour
{
    public int gunDamage = 1;
    public float gunFireRate = 0.25f;
    public float gunRange = 50f;
    public float gunHitForce = 100f;

    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds gunRayDuration = new WaitForSeconds(.07f);
    private AudioSource gunAudio;
    private LineRenderer laser;
    private float nextShot;

    void Start()
    {
        laser = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
    }

    void Update()
    {
        if (Input.GetButtonDown ("Fire1") && Time.time > nextShot)
        {
            nextShot = Time.time + gunFireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laser.SetPosition(0, gunEnd.position);

            // Out is used to output and return an additional variable from this function
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, gunRange))
            {
                laser.SetPosition(1, hit.point);
                ShootableBox box = hit.collider.GetComponent<ShootableBox>();
                if (box != null)
                {
                    box.Damage(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(fpsCam.transform.forward * gunHitForce);
                }

            }
            else
            {
                laser.SetPosition(1, rayOrigin + (fpsCam.transform.forward * gunRange));
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laser.enabled = true;
        yield return gunRayDuration;
        laser.enabled = false;
    }
}
