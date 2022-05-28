using System.Collections;
using UnityEngine;

namespace Vongrid.DemoFPS.Demo
{
    public class Dummy : MonoBehaviour
    {
        [SerializeField]
        private float respawnTime = 2f;

        [SerializeField]
        private float animationDuration = 0.5f;

        private Transform dummyTrasnform;

        private bool hit;

        private void Start()
        {
            dummyTrasnform = transform;
        }

        public void Hit()
        {
            if (hit)
            {
                return;
            }

            hit = true;
            StopAllCoroutines();
            SmoothRotate(90);
            Invoke(nameof(Reset), respawnTime);
        }

        private void Reset()
        {
            SmoothRotate(0);
            hit = false;
        }

        private void SmoothRotate(float angle)
        {
            StartCoroutine(SmoothRotateCoroutine(angle, animationDuration));
        }

        public IEnumerator SmoothRotateCoroutine(float targetAngle, float duration)
        {
            Quaternion rotation = dummyTrasnform.rotation;

            Vector3 targetEulerRotation = rotation.eulerAngles;
            targetEulerRotation.z = targetAngle;
            Quaternion targetRotation = Quaternion.Euler(targetEulerRotation);

            float currentTime = 0.0f;
            while (currentTime < duration)
            {
                dummyTrasnform.rotation = Quaternion.Lerp(rotation, targetRotation, currentTime / duration);

                currentTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
