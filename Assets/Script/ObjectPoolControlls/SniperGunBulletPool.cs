using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperGunBulletPool : MonoBehaviour {

    public GameObject PooledSniperBullet;
    public int PooledAmount;
    public bool CanGrow;
    private GameObject PooledObj;
    private List<GameObject> SniperBulletList;
	// Use this for initialization
	void Start () {
        SniperBulletList = new List<GameObject>();
		for(int i=0; i < PooledAmount; i++)
        {
            PooledObj = (GameObject)Instantiate(PooledSniperBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            SniperBulletList.Add(PooledObj);
        }
	}
	
    public GameObject GetSniperGunBullet()
    {
        for(int i=0; i < SniperBulletList.Count; i++)
        {
            if (!SniperBulletList[i].activeInHierarchy)
            {
                return SniperBulletList[i];
            }
        }
        if (CanGrow)
        {
            PooledObj = (GameObject)Instantiate(PooledSniperBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            SniperBulletList.Add(PooledObj);
            return PooledObj;
        }
        return null;
    }
	
}
