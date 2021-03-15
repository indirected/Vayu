using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotationLimit : MonoBehaviour {
    public float MaxXRotation;
    public float MinXRotation;
    public float MaxZRotation;
    public float MinZRotation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (transform.rotation.x > MaxXRotation)
        {
            transform.rotation = Quaternion.Euler( MaxXRotation, 0, 0);
        }
        else if (transform.rotation.x < MinXRotation)
        {
            transform.rotation = Quaternion.Euler( MinXRotation , 0, 0);
        }
        else if ( transform.rotation.z > MaxZRotation)
        {
            transform.rotation = Quaternion.Euler(0, 0, MaxZRotation);
        }
        else if (transform.rotation.z < MinZRotation)
        {
            transform.rotation = Quaternion.Euler( 0 , 0 , MinZRotation);
        }
	}
}
