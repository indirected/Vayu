using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarWheelsControl : MonoBehaviour {
    public WheelCollider[] FrontWheels = new WheelCollider[2];
    public WheelCollider[] RearWheels = new WheelCollider[2];
    public bool IsTruning;
    private float MaxSpeed;
    // Update is called once per frame
    private void Start()
    {
        MaxSpeed = GetComponent<CarController>().m_Topspeed;
    }
    void Update () {
        var Speed = GetComponent<Rigidbody>().velocity.magnitude;
        if(Mathf.Floor(Speed) > 45 && Input.GetAxis("Vertical") > 0)
        {
            ForwartTurningOnSpeed();
        }else
        {
            ResetRearWheels();
        }
        if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") != 0)
        {

            BackwardTurning();
            IsTruning = true;
        }
        else
        {
            ReseFrontWheels();
            IsTruning = false;
        }
	}
    public void ForwartTurningOnSpeed()
    {
        foreach(var item in RearWheels)
        {
            var sfriction = item.sidewaysFriction;
            sfriction.stiffness = 1.5f;
            item.sidewaysFriction = sfriction;
        }
    }
    public void BackwardTurning()
    {
        foreach(var item in FrontWheels)
        {
            var sfriction = item.sidewaysFriction;
            sfriction.stiffness = 3f;
            sfriction.asymptoteSlip = 1.5f;
            item.sidewaysFriction = sfriction;
        }
        GetComponent<CarController>().m_SteerHelper = .4f;
    }
    public void ResetRearWheels()
    {
        foreach (var item in RearWheels)
        {
            var sfriction = item.sidewaysFriction;
            sfriction.stiffness = 1f;
            item.sidewaysFriction = sfriction;
        }

    }
    public void ReseFrontWheels()
    {
        foreach (var item in FrontWheels)
        {
            var sfriction = item.sidewaysFriction;
            sfriction.stiffness = 1f;
            sfriction.asymptoteSlip = 1.5f;
            item.sidewaysFriction = sfriction;
        }
        GetComponent<CarController>().m_SteerHelper = .75f;
    }
}
