using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBulletPool : MonoBehaviour {

    public GameObject RPGBullet;
    public int PooledAmount = 0;
    public bool CanGrow = true;
    private GameObject PooledObj;
    private List<GameObject> RPGBulletList;

    // Use this for initialization
    void Start()
    {
        RPGBulletList = new List<GameObject>();
        for (int i = 0; i < PooledAmount; i++)
        {
            PooledObj = (GameObject)Instantiate(RPGBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            RPGBulletList.Add(PooledObj);
        }
    }

    public GameObject GetRPGBullet()
    {
        for (int i = 0; i < RPGBulletList.Count; i++)
        {
            if (!RPGBulletList[i].activeInHierarchy)
            {
                return RPGBulletList[i];
            }
        }
        if (CanGrow)
        {
            PooledObj = (GameObject)Instantiate(RPGBullet, Vector3.zero, Quaternion.identity);
            PooledObj.SetActive(false);
            RPGBulletList.Add(PooledObj);
            return PooledObj;
        }
        return null;
    }
}
