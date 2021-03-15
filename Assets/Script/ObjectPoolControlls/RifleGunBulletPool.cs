using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGunBulletPool : MonoBehaviour {

    public GameObject PooledRifleBullet;
    public int PooledAmount;
    public bool CanGrow;
    private GameObject pooledobj;
    private List<GameObject> RifleGunBulletList;

	// Use this for initialization
	void Start () {
        RifleGunBulletList = new List<GameObject>();
        for (int i=0; i < PooledAmount; i++)
        {
            pooledobj = (GameObject)Instantiate(PooledRifleBullet, Vector3.zero, Quaternion.identity);
            pooledobj.SetActive(false);
            RifleGunBulletList.Add(pooledobj);
        }
	}
	
	public GameObject GetRifleBullet()
    {
        for(int i=0; i < RifleGunBulletList.Count; i++)
        {
            if (!RifleGunBulletList[i].activeInHierarchy)
            {
                return RifleGunBulletList[i];
            }
        }
        if (CanGrow)
        {
            pooledobj = (GameObject)Instantiate(PooledRifleBullet, Vector3.zero, Quaternion.identity);
            pooledobj.SetActive(false);
            RifleGunBulletList.Add(pooledobj);
            return pooledobj;
        }
        return null;
    }
}
