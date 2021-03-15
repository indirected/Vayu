using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class RampCharge : MonoBehaviour {

    public int charge;
    public float UnlimitSpeedTime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //charges the car on ramp
    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        other.GetComponentInParent<CarController>().RampBoost(charge);
        StartCoroutine(DisBoost(UnlimitSpeedTime, other.gameObject));
    }
    IEnumerator DisBoost(float Delay , GameObject Object)
    {
        yield return new WaitForSeconds(Delay);
        Object.GetComponentInParent<CarController>().IsBoost = false;
    }
}
