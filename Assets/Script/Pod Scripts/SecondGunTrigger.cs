﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGunTrigger : MonoBehaviour {

    private GameObject PodControllerObj;
    public int PodPositionNum;
    GameObject other;

    // Use this for initialization
    void Start () {
        PodControllerObj = FindObjectOfType<GunPodSpawnControll>().gameObject;
    }
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision on Secondary weapon pod");
        try
        {
            other = collider.gameObject.transform.parent.transform.parent.gameObject;
            if (other.tag == "Player")
            {

                var PlayerLevel = other.GetComponent<Level>().PlayerLevel;
                var RequiredLevel = other.GetComponent<WeaponPickingSystem>().SmgGunLevel;
                if (PlayerLevel >= RequiredLevel)
                {
                    other.GetComponent<WeaponPickingSystem>().ActivateSecondGun();
                    PodControllerObj.GetComponent<GunPodSpawnControll>().CurrentSecondGuns--;
                    PodControllerObj.GetComponent<GunPodSpawnControll>().MakePodAvailable(PodPositionNum);
                    gameObject.SetActive(false);
                }
            }
        }
        catch { }              
    }
    
    
    // Update is called once per frame

}
