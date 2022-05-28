using System.Collections;
using TMPro;
using UnityEngine;

namespace Vongrid.DemoFPS.Demo
{
    public class GunController : MonoBehaviour
    {
        [SerializeField]
        private Gun gunInfo;
        [SerializeField]
        private ParticleSystem muzzleFlash;
        [SerializeField]
        private GameObject impactEffect;
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private TextMeshProUGUI bulletsUI;
        [SerializeField]
        private LayerMask targetLayers;

        private int bulletsLeft;
        private float nextTimeToShoot;
        private bool isReloading;
        private Animator animator;

        private void Start()
        {
            bulletsLeft = gunInfo.magSize;
            animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateGunState();
            bulletsUI.text = $"{bulletsLeft}/{gunInfo.magSize}";
        }

        private void UpdateGunState()
        {
            if (isReloading)
            {
                return;
            }

            if ((Input.GetKeyDown(KeyCode.R) && bulletsLeft < gunInfo.magSize) || bulletsLeft <= 0)
            {
                Reload();
                return;
            }

            if (Input.GetMouseButton(0) && Time.time >= nextTimeToShoot)
            {
                nextTimeToShoot = Time.time + (1f / gunInfo.fireRate);
                Shoot();
            }
        }

        private void Shoot()
        {
            muzzleFlash.Play();

            bool wasObjectHit = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, gunInfo.range, targetLayers);

            bulletsLeft--;

            if (wasObjectHit)
            {
                // Add the hit normal multiplied by a small value so it's on top of the object and not inside it
                Vector3 impactLocation = hit.point + (hit.normal * 0.01f);
                GameObject impactObject = Instantiate(impactEffect, impactLocation, Quaternion.FromToRotation(Vector3.back, hit.normal));
                impactObject.transform.SetParent(hit.transform);
                Destroy(impactObject, 20f);

                hit.transform.gameObject.GetComponent<Dummy>()?.Hit();

                // Dummy dummyHit = hit.transform.gameObject.GetComponent<Dummy>();
                // if (dummyHit != null)
                // {
                //     dummyHit.Hit();
                // }
            }
        }

        private void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            isReloading = true;

            SetReloadAnimationSpeed();

            yield return new WaitForSeconds(gunInfo.reloadTime);

            bulletsLeft = gunInfo.magSize;
            isReloading = false;
        }

        private void SetReloadAnimationSpeed()
        {
            float animationSpeed = animator.runtimeAnimatorController.animationClips[0].length / gunInfo.reloadTime;
            animator.SetFloat("ReloadSpeed", animationSpeed);
            animator.SetTrigger("Reload");
        }
    }
}
