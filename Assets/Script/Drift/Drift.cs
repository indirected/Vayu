using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour {
    public WheelCollider[] Backwheels;
    public WheelCollider[] FrontWheels;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<MovingOptions>().Drifting == true)
        {
            SetStiffness(.6f);
            foreach (var i in FrontWheels)
            {
                WheelFrictionCurve sfriction = i.sidewaysFriction;
                sfriction.stiffness = 1.4f;
                i.sidewaysFriction = sfriction;
            }
        }
        else
        {
            SetStiffness(1f);
        }
        
	}

    void SetStiffness(float Value)
    {
        foreach (var item in Backwheels)
        {

            WheelFrictionCurve sfriction = item.sidewaysFriction;
            sfriction.stiffness = Value;
            item.sidewaysFriction = sfriction;
            /*if (item.name == "RightBack" || item.name == "LeftBack")
            {
                

            }*/

        }
        
    }
}
