using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FXV
{
    public class FXVRotate : MonoBehaviour
    {

        public Vector3 rotationSpeed = Vector3.up;

        private Vector3 currentRotation;
        private Vector3 originRotation;

        void Start()
        {
            currentRotation = transform.rotation.eulerAngles;
            originRotation = transform.rotation.eulerAngles;
        }

        void Update()
        {
            currentRotation += rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.identity;
            transform.Rotate(currentRotation);
        }
        public void RenewRotation(Vector3 rot)
        {
            currentRotation = rot;
        }
        public void SetDefaultRotation()
        {
            currentRotation = originRotation;
        }
    }
}
