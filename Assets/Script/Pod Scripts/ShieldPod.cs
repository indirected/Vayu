using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPod : MonoBehaviour {


    public int RequiredLevel;
    private GameObject PodControllerObj;
    public int PodPositionNum;
    Transform Object;
    Level level;
    private void Start()
    {
        PodControllerObj = FindObjectOfType<GunPodSpawnControll>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Object = other.gameObject.transform.parent.transform.parent;
            if (Object == null) return;
            level = Object.GetComponent<Level>();
            if (Object.tag == "Player" && level.PlayerLevel >= RequiredLevel)
            {
                Object.GetComponent<Shield>().EnableShield();
                PodControllerObj.GetComponent<GunPodSpawnControll>().CurrentShields--;
                PodControllerObj.GetComponent<GunPodSpawnControll>().MakePodAvailable(PodPositionNum);
                gameObject.SetActive(false);
            }
        }
        catch { }
    }

}
