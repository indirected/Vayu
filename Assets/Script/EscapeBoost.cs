using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;


public class EscapeBoost : NetworkBehaviour {

    public int BoostForce;
    public float UnlimitSpeedTimetoEscape;
    public float WaitBeforeRecharge;
    public KeyCode BoostKey;
    public bool BoostEnded;
    private Slider BoostSlider;
    private bool CanBoost;
    public float StopSpeed;
	// Use this for initialization
	void Start () {
        BoostSlider = GameObject.Find("BoostSlider").GetComponent<Slider>();
        BoostSlider.maxValue = UnlimitSpeedTimetoEscape;
        BoostSlider.value = UnlimitSpeedTimetoEscape;
        CanBoost = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(BoostKey) && Input.GetAxisRaw("Vertical") == 1 && CanBoost)
        {
            //gameObject.GetComponent<CarController>().EscapeBoost(BoostForce);


            StartCoroutine(BoostDelayTime());
        }
	}

    //To unlock 
    IEnumerator BoostDelayTime()
    {
        for (float i = UnlimitSpeedTimetoEscape; i >= 0; i = i - 0.1f)
        {
            var localvel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
            int Direction = new int();
            BoostSlider.value = i;
            if (localvel.z > .5f) Direction = 1;
            else if (localvel.z < -.5f) Direction = -1;
            gameObject.transform.Translate(transform.forward * Direction  , Space.World);
            //gameObject.GetComponent<CarController>().rig.AddForce(transform.forward * -1 * StopSpeed);
            yield return new WaitForSecondsRealtime(0.01f);
            
        }
        gameObject.GetComponent<CarController>().IsBoost = false;
        BoostSlider.value = 0;
        CanBoost = false;
        yield return new WaitForSeconds(WaitBeforeRecharge);
        StartCoroutine(BoostRecharge());
    }
    IEnumerator BoostRecharge()
    {
        for (float i = 0; i <= BoostSlider.maxValue; i = i + 0.01f)
        {
            BoostSlider.value = i;
            yield return new WaitForSeconds(0.01f);
            
        }
        CanBoost = true;
    }

}
