using UnityEngine;

public class Weapon : MonoBehaviour {
    public float damage = 10f;
    public float range = 200f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float timeToFire = 0f;

    void Update() {
        // full auto:
        //if (Input.GetButton("Fire1") && Time.time >= timeToFire) {
        if (Input.GetButtonDown("Fire1") && Time.time >= timeToFire) {
            timeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot() {
        muzzleFlash.Play();

        //using raycasting
        RaycastHit hit;

        // origin, direction, write into, range
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {
            Debug.Log(hit.transform);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(hit.normal * impactForce);
            }

            GameObject impactGameObject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }
    }
}
