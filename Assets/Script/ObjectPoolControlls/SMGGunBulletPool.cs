using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGGunBulletPool : MonoBehaviour {

    public GameObject PooledSMGBullet;
    public int PooledAmount = 15;
    public bool CanGrow = true;
    private GameObject Pooledobj,PooledObj;
    List<GameObject> SMGBulletList;

	// Use this for initialization
	void Start () {
        SMGBulletList = new List<GameObject>();
        for(int i=0; i<PooledAmount; i++)
        {
            Pooledobj = (GameObject)Instantiate(PooledSMGBullet, new Vector3 (0,0,0) , Quaternion.identity);
            Pooledobj.SetActive(false);
            SMGBulletList.Add(Pooledobj);
        }
	}
	

    public GameObject GetSMGGunBullet()
    {
        for(int i=0; i< SMGBulletList.Count; i++)
        {
            if (!SMGBulletList[i].activeInHierarchy)
            {
                return SMGBulletList[i];
            }
        }
        if (CanGrow)
        {
            PooledObj = (GameObject)Instantiate(PooledSMGBullet);
            PooledObj.SetActive(false);
            SMGBulletList.Add(PooledObj);
            return PooledObj;
        }
        return null;
    }
	
}
