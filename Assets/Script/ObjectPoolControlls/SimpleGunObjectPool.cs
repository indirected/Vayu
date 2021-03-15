using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGunObjectPool : MonoBehaviour {

    public GameObject PooledObject;
    public int PooledAmount = 10;
    public bool CanGrow = true;
    private GameObject Pooledobj, PooledObj;

    List<GameObject> PooledObjectsList;

	// Use this for initialization
	void Start () {
        PooledObjectsList = new List<GameObject>();
        for(int i =0; i < PooledAmount; i++)
        {
            Pooledobj = (GameObject)Instantiate(PooledObject, new Vector3(0,0,0) , Quaternion.identity);
            Pooledobj.SetActive(false);
            PooledObjectsList.Add(Pooledobj);
        }
	}
    


    public GameObject GetPooledObject()
    {
        for (int i=0; i < PooledObjectsList.Count; i++)
        {
            if (!PooledObjectsList[i].activeInHierarchy)
            {
                return PooledObjectsList[i];
            }
        }

        if (CanGrow)
        {
            PooledObj = (GameObject)Instantiate(PooledObject);
            
            PooledObj.SetActive(false);
            PooledObjectsList.Add(PooledObj);
            return PooledObj;
        }

        return null;
    }
   
}
