using UnityEngine;

public class WeaponRevisited : MonoBehaviour {
    public float damage = 10f;
    public float range = 200f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public float projectileSpeed = 10f;
    public float reloadTime = 1f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject prefabProjectile;
    public Transform weaponMuzzle;

    private float timeToFire = 0f;
    private int rounds = 0;
    private float timeToNextProjectileReady = 0f;

    private void Reset() {
        rounds = 1;
        // TODO change to rate? 1f/reloadTime
        timeToNextProjectileReady = RefillTime(reloadTime);
    }

    void Update() {
        if (Input.GetButtonDown("Fire1") && Time.time >= timeToFire) {
            ShootProjectile();
        }

        timeToNextProjectileReady = RefillTime(reloadTime);
    }

    private void ShootProjectile() {
        muzzleFlash.Play();

        //if (rounds > 0) {
        // https://www.studica.com/blog/how-to-create-a-projectile-in-unity
        GameObject projectile = Instantiate(prefabProjectile, weaponMuzzle.position, weaponMuzzle.rotation);
        Rigidbody rigidBodyProjectile = projectile.GetComponent<Rigidbody>();
        rigidBodyProjectile.AddForce(weaponMuzzle.forward * projectileSpeed, ForceMode.VelocityChange);
        //}

        rounds -= 1;
        timeToNextProjectileReady = RefillTime(reloadTime);

        switch (rounds) {
            default:
                break;
        }
    }

    private float RefillTime(float timeToRefill) {
        return Time.time + timeToRefill;
    }
}
