using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour {
    public float recoil = 1f;
    public Transform barrel;
    public GameObject projectilePrefab;

    private XRGrabInteractable interactable;
    private Rigidbody rigidbody;


    private void Awake() {
        interactable = GetComponent<XRGrabInteractable>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        interactable.onActivate.AddListener(Fire);
    }

    private void OnDisable() {
        interactable.onActivate.RemoveListener(Fire);
    }

    private void Fire(XRBaseInteractor interactor) {
        print("Pew");
        CreateProjectile();
        ApplyRecoil();
    }

    private void CreateProjectile() {
        var projectileObject = Instantiate(projectilePrefab, barrel.position, barrel.rotation);
        var projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch();
    }

    private void ApplyRecoil() {
        rigidbody.AddRelativeForce(Vector3.right * recoil, ForceMode.Impulse);
    }
}
