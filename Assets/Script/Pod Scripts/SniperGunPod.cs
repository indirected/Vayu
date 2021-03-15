using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGunPod : MonoBehaviour
{

    private GameObject PodControllerObj;
    public int PodPositionNum;

    // Use this for initialization
    void Start()
    {
        PodControllerObj = FindObjectOfType<GunPodSpawnControll>().gameObject;
    }
    Transform other;
    int PlayerLevel;
    int RequiredLevel;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision on Sniper Gun pod");
        try
        {
            other = collider.gameObject.transform.parent.transform.parent;
            if (other.tag == "Player")
            {

                PlayerLevel = other.GetComponent<Level>().PlayerLevel;
                RequiredLevel = other.GetComponent<WeaponPickingSystem>().SniperGunLevel;
                if (PlayerLevel >= RequiredLevel)
                {
                    other.GetComponent<WeaponPickingSystem>().ActivateSniperGun();
                    PodControllerObj.GetComponent<GunPodSpawnControll>().CurrentSniperGuns--;
                    PodControllerObj.GetComponent<GunPodSpawnControll>().MakePodAvailable(PodPositionNum);
                    gameObject.SetActive(false);
                }
            }
        }
        catch { }

    }
}
