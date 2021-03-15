using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        Rigidbody rig;
        public int Accelaration;
        WheelCollider[] wh;
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            rig = GetComponent<Rigidbody>();
            wh = GetComponentsInChildren<WheelCollider>();
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            var steer = new int();
            if ( h > 0) steer = 1;
            else if (h < 0) steer = -1;
            else steer = 0;
#if !MOBILE_INPUT
                //float handbrake = CrossPlatformInputManager.GetAxis("Jump");
                m_Car.Move(steer, v * Mathf.Infinity, v, 0f);
            if(wh[1].isGrounded || wh[0].isGrounded || wh[2].isGrounded || wh[3].isGrounded)
            rig.AddForce(transform.forward * Time.fixedDeltaTime * (v * Accelaration));
#else
            m_Car.Move(steer, v, v, 0f);
#endif
        }
    }
}
