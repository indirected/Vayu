using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleGunPod : MonoBehaviour {

    public int PodPositionNum;
    private GameObject PodControllerObj;
    Transform other;
    WeaponPickingSystem WeaponPickingScript;
    WeaponPickingSystem.GUN_TYPE CurrentGun;
    // Use this for initialization
    void Start () {
        PodControllerObj = FindObjectOfType<GunPodSpawnControll>().gameObject;
	}
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collision on Secondary weapon pod");
        try
        {
            other = collider.gameObject.transform.parent.transform.parent;
            if (other.tag == "Player")
            {

                WeaponPickingScript = other.GetComponent<WeaponPickingSystem>();
                
                other.GetComponent<ShootControll>().PrimarySlotMagzineLeft = WeaponPickingScript.SGMagzine;
                CurrentGun = WeaponPickingScript.WhichGunisOn;
                if(CurrentGun == WeaponPickingSystem.GUN_TYPE.simpleGun)
                {
                    other.GetComponent<ShootControll>().MagzineLeftText.text = WeaponPickingScript.SGMagzine.ToString();
                    other.GetComponent<ShootControll>().CurrentMagzine = WeaponPickingScript.SGMagzine;
                }
                PodControllerObj.GetComponent<GunPodSpawnControll>().CurrentSimpleGuns--;
                PodControllerObj.GetComponent<GunPodSpawnControll>().MakePodAvailable(PodPositionNum);
                gameObject.SetActive(false);

            }
            //if (other == null) return;
        }
        catch { }
    }

    

    // Update is called once per frame
    void Update () {
		
	}
}
