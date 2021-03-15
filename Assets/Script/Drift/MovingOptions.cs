using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// How To Use : 
/// 1- Use The Names Of Wheels like " LeftFront , RightFront , LeftBack , RightBack"
/// 2- Use TorqueSpeed For Forward And Backward
/// 3- Use Rotate Speed For Left And Right 
/// 4- Add This Script To Car GameObject
/// 5- For Using Drift Edit " RotateTime , And Drifting bool " & Drift.cs & Wheel Effects
/// </summary>


public class MovingOptions : MonoBehaviour {
    public WheelCollider[] wheels;
    public float TorqueSpeed;
    public float RotateSpeed;
    public int MaxSpeed;
    public int AutoBrakeTorque;
    public int BrakeTorque;
    // Used in Drift.cs & WheelEffect.cs Script
    public bool Drifting = false;
    private int RotateTime;
    public bool IsStopping;
    
	// Use this for initialization
	void Start () {
        wheels = GetComponentsInChildren<WheelCollider>();       
	}

    // Update is called once per frame
    void FixedUpdate() {
        Rigidbody rig = GetComponent<Rigidbody>();
        var speed = rig.velocity.magnitude;
        print(speed);
        var velocity = rig.velocity;
        var localVel = transform.InverseTransformDirection(velocity);
        if (speed >= MaxSpeed)
        {
            foreach (var item in wheels)
            {
                item.motorTorque = 0;
                rig.velocity = MaxSpeed * rig.velocity.normalized;
            }
        }
        var Rotation = transform.rotation.z;
        if (Rotation >= 10) transform.Rotate(0,0,10);
        //print(transform.rotation.z);
        // Checking For Vertical Buttons And Going to Forward Or Back
        var x = Input.GetAxis("Vertical");
        if (x == 1 || x == -1)
        {
            Forward(x);
        }
        // Stop Moving After Buttons Unpressed
        else if (wheels[0].motorTorque != 0 || wheels[1].motorTorque != 0 || wheels[2].motorTorque != 0 || wheels[3].motorTorque != 0 && x == 0)
        {
            foreach (var item in wheels)
            {
                item.brakeTorque = AutoBrakeTorque;
            }

        }
        else if (localVel.z > 0 && x < 0)
        {
            foreach (var item in wheels)
            {
                item.brakeTorque = BrakeTorque;
            }
        }
        else if (localVel.z < 0 && x > 0)
        {
            foreach (var item in wheels)
            {
                item.brakeTorque = BrakeTorque;
            }
        }
        /*else if (wheels[0].motorTorque < 0 || wheels[1].motorTorque < 0 || wheels[2].motorTorque < 0 || wheels[3].motorTorque < 0)
        {
            foreach (var item in wheels)
            {
                item.motorTorque += 20;

            }

        }
        // Khate Tormoz 
        /*if (wheels[0].motorTorque < 0 || wheels[1].motorTorque < 0 || wheels[2].motorTorque < 0 || wheels[3].motorTorque < 0 && Input.GetAxis("Vertical") < 0)
        {
            IsStopping = true;
            Drifting = true;
        }
        else if(wheels[0].motorTorque > 0 || wheels[1].motorTorque > 0 || wheels[2].motorTorque > 0 || wheels[3].motorTorque > 0 && Input.GetAxis("Vertical") > 0)
        {
            IsStopping = true;
            Drifting = true;
        }
        if(IsStopping && wheels[0].motorTorque == 0 || wheels[1].motorTorque == 0 || wheels[2].motorTorque == 0 || wheels[3].motorTorque == 0)
        {
            IsStopping = false;
            Drifting = false;
        }*/
        // Checking Horizontal Buttons For Rotating
        var y = Input.GetAxis("Horizontal");
        if (y != 0)
        {
            Rotating(y);
            RotateTime++;
        }
        // Stop Rotating After Buttons Unpressed
        else if (wheels[0].steerAngle > 0 || wheels[1].steerAngle > 0 || wheels[2].steerAngle > 0 || wheels[3].steerAngle > 0)
        {
            foreach (var item in wheels)
            {
                    item.steerAngle = 0;

            }
            RotateTime = 0;
        }
        else if (wheels[0].steerAngle < 0 || wheels[1].steerAngle < 0 || wheels[2].steerAngle < 0 || wheels[3].steerAngle < 0)
        {
            foreach (var item in wheels)
            {
                    item.steerAngle = 0;
            }
            RotateTime = 0;
        }
        if (RotateTime > 30 && Input.GetAxis("Vertical") != 0) Drifting = true;
        else Drifting = false;
        
        //print(Input.GetAxis("Vertical"));
    }
    // Moving Options ===> i For Understanding Moving Forward Or Backward
    public void Forward(float i)
    {
        
        foreach(var item in wheels)
        {
            item.brakeTorque = 0;
            item.motorTorque =  i * TorqueSpeed;
        }
        
    }
    // Rotate Options === i For Understanding Left Or Right
    public void Rotating(float i)
    {
        foreach(var item in wheels)
        {
            if(item.name == "LeftFront" || item.name == "RightFront")
            {
                item.steerAngle = i * RotateSpeed;
            }
            else
            {
                item.steerAngle = 0;
            }
        }
    }
}
