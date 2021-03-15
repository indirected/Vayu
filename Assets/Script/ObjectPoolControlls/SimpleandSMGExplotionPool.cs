using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleandSMGExplotionPool : MonoBehaviour {

    public GameObject PooledObject;
    public int PooledAmount = 3;
    public bool CanGrow = true;
    private List<GameObject> SimpleExplotionList;
    private GameObject Pooledobj;


	// Use this for initialization
	void Start () {
        SimpleExplotionList = new List<GameObject>();
        for (int i=0; i < PooledAmount; i++)
        {
            Pooledobj = (GameObject)Instantiate(PooledObject, Vector3.zero, Quaternion.identity);
            Pooledobj.SetActive(false);
            SimpleExplotionList.Add(Pooledobj);
        }

	}

    

    public GameObject GetSimpleExplotion()
    {
        for(int i=0; i < SimpleExplotionList.Count; i++)
        {
            if (!SimpleExplotionList[i].activeInHierarchy)
            {
                return SimpleExplotionList[i];
            }
        }
        if (CanGrow)
        {
            Pooledobj = (GameObject)Instantiate(PooledObject, Vector3.zero, Quaternion.identity);
            Pooledobj.SetActive(false);
            //else CmdNEtworkSpawn(Pooledobj);
            SimpleExplotionList.Add(Pooledobj);
            return Pooledobj;
        }
        return null;
    }
	
	
}
