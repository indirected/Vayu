using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutGunBulletPool : MonoBehaviour {

    public GameObject ShutGunBullet;
    public int PooledAmount = 0;
    public bool CanGrow = true;
    private GameObject PooledObj;
    private List<GameObject> ShutGunBulletList;

	// Use this for initialization
	void Start () {
        ShutGunBulletList = new List<GameObject>();
        for (int i=0; i < PooledAmount; i++)
        {
            PooledObj = (GameObject)Instantiate(ShutGunBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            ShutGunBulletList.Add(PooledObj);
        }
	}
	
    public GameObject GetShutGunBullet()
    {
        for (int i=0; i < ShutGunBulletList.Count; i++)
        {
            if (!ShutGunBulletList[i].activeInHierarchy)
            {
                return ShutGunBulletList[i];
            }
        }
        if (CanGrow)
        {
            PooledObj = (GameObject)Instantiate(ShutGunBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            ShutGunBulletList.Add(PooledObj);
            return PooledObj;
        }
        return null;
    }
	
}
