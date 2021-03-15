using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGPod : MonoBehaviour
{

    private GameObject PodControllerObj;
    public int PodPositionNum;
    GameObject other;
    // Use this for initialization
    void Start()
    {
        PodControllerObj = FindObjectOfType<GunPodSpawnControll>().gameObject;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision on RPG pod");
        try
        {
            other = collider.gameObject.transform.parent.transform.parent.gameObject;
            if (other.tag == "Player")
            {

                var PlayerLevel = other.GetComponent<Level>().PlayerLevel;
                var RequiredLevel = other.GetComponent<WeaponPickingSystem>().RPGLevel;
                if (PlayerLevel >= RequiredLevel)
                {
                    other.GetComponent<WeaponPickingSystem>().ActivateRPG();
                    PodControllerObj.GetComponent<GunPodSpawnControll>().CurrentRPGs--;
                    PodControllerObj.GetComponent<GunPodSpawnControll>().MakePodAvailable(PodPositionNum);
                    gameObject.SetActive(false);
                }
            }
        }
        catch { }

    }
}
