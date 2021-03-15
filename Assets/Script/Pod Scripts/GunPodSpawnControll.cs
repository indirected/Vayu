using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunPodSpawnControll : NetworkBehaviour {

    //Posotion Variables
    //public Vector3 Center;
    //public Vector3 Size;
    //public float ObstacleRadius;
    public PodSpawnPosition[] PodPositions;
    private int whichpodchosed;
    
    //SimpleGunPod Variables
    public int AvailableSimpleGuns;
    public GameObject SimpleGunPod;
    [HideInInspector]
    public int CurrentSimpleGuns;
    private List<GameObject> SimpleGunPodList;


    //SecondGunPod Variables
    public int AvailableSecondGuns;
    public GameObject SecondGunPod;
    [HideInInspector]
    public int CurrentSecondGuns;
    private List<GameObject> SecondGunPodList;

    //RifleGunPod Variables
    public int AvailableRifleGuns;
    public GameObject RifleGunPod;
    [HideInInspector]
    public int CurrentRifleGuns;
    private List<GameObject> RifleGunPodList;

    //SniperGunPod Variables
    public int AvailableSniperGuns;
    public GameObject SniperGunPod;
    [HideInInspector]
    public int CurrentSniperGuns;
    private List<GameObject> SniperGunPodList;

    //ShutGun Pod Variables
    public int AvailableShutGuns;
    public GameObject ShutGunPod;
    [HideInInspector]
    public int CurrentShutGuns;
    private List<GameObject> ShutGunPodList;

    //RPG Pod Variables
    public int AvailableRPGs;
    public GameObject RPGPod;
    [HideInInspector]
    public int CurrentRPGs;
    private List<GameObject> RPGPodList;


    //ShieldPod Variables
    public int AvailableShields;
    public GameObject ShieldPod;
    [HideInInspector]
    public int CurrentShields;
    private List<GameObject> ShieldPodList;

    // Use this for initialization
    void Start () {
        SimpleGunPodList = new List<GameObject>();
        SecondGunPodList = new List<GameObject>();
        RifleGunPodList = new List<GameObject>();
        ShieldPodList = new List<GameObject>();
        SniperGunPodList = new List<GameObject>();
        ShutGunPodList = new List<GameObject>();
        RPGPodList = new List<GameObject>();
        PodPositions = FindObjectsOfType<PodSpawnPosition>();
        if (isServer) StartCoroutine(CreatePods());
	}

  

    /*public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Center, Size);
    }*/
    IEnumerator CreatePods()
    {
        while (enabled)
        {
            if (CurrentSimpleGuns < AvailableSimpleGuns)
            {
                SrvSimpleGun(SetPodPos());
                //CurrentSimpleGuns++;
            }

            if(CurrentSecondGuns < AvailableSecondGuns)
            {
                SrvSecondGun(SetPodPos());
                //CurrentSecondGuns++;
            }

            if(CurrentShields < AvailableShields)
            {
                SrvShield(SetPodPos());
                //CurrentShields++;
            }
            if(CurrentRifleGuns < AvailableRifleGuns)
            {
                SrvRifleGun(SetPodPos());
            }
            if(CurrentSniperGuns < AvailableSniperGuns)
            {
                SrvSniperGun(SetPodPos());
            }
            if(CurrentShutGuns < AvailableShutGuns)
            {
                SrvShutGun(SetPodPos());
            }
            if(CurrentRPGs < AvailableRPGs)
            {
                SrvRPG(SetPodPos());
            }
            yield return new WaitForSeconds(1);
        }
    }


    
    // Network 
    [ClientRpc]
    void RpcSetPos(GameObject obj, Vector3 pos)
    {
        obj.transform.position = pos;
    }

    [ClientRpc]
    void RpcShield(GameObject go , int Val)
    {
        go.GetComponent<ShieldPod>().PodPositionNum = Val;
        go.SetActive(true);
        //CurrentShields++;
    }
    [ClientRpc]
    void RpcSecondGun(GameObject go, int Val)
    {
        go.GetComponent<SecondGunTrigger>().PodPositionNum = Val;
        go.SetActive(true);
        //CurrentSecondGuns++;
    }
    [ClientRpc]
    void RpcSimpleGun(GameObject go, int Val)
    {
        go.GetComponent<SimpleGunPod>().PodPositionNum = Val;
        go.SetActive(true);
        //CurrentSimpleGuns++;
    }
    [ClientRpc]
    void RpcRifleGun(GameObject go, int Val)
    {
        go.GetComponent<RifleGunPod>().PodPositionNum = Val;
        go.SetActive(true);
        //CurrentRifleGuns++;
    }
    [ClientRpc]
    void RpcSniperGun(GameObject go,int Val)
    {
        go.GetComponent<SniperGunPod>().PodPositionNum = Val;
        go.SetActive(true);
    }
    [ClientRpc]
    void RpcShutGun(GameObject go, int Val)
    {
        go.GetComponent<ShutGunPod>().PodPositionNum = Val;
        go.SetActive(true);
    }
    [ClientRpc]
    void RpcRPG(GameObject go, int Val)
    {
        go.GetComponent<RPGPod>().PodPositionNum = Val;
        go.SetActive(true);
    }


    [Server]
    void SrvShield(Vector3 Pos)
    {
        var shieldpod = GetShieldPod();
        RpcSetPos(shieldpod, Pos);
        RpcShield(shieldpod, whichpodchosed);
        CurrentShields++;
    }
    [Server]
    void SrvSecondGun(Vector3 Pos)
    {
        var secondpod = GetSecondGunPod();
        RpcSetPos(secondpod, Pos);
        RpcSecondGun(secondpod, whichpodchosed);
        CurrentSecondGuns++;
    }
    [Server]
    void SrvSimpleGun(Vector3 Pos)
    {
        var simplepod = GetSimplePod();
        RpcSetPos(simplepod, Pos);
        RpcSimpleGun(simplepod, whichpodchosed);
        CurrentSimpleGuns++;
    }
    [Server]
    void SrvRifleGun(Vector3 Pos)
    {
        var riflepod = GetRifleGunPod();
        RpcSetPos(riflepod, Pos);
        RpcRifleGun(riflepod, whichpodchosed);
        CurrentRifleGuns++;
    }
    [Server]
    void SrvSniperGun(Vector3 Pos)
    {
        var sniperpod = GetSniperGunPod();
        RpcSetPos(sniperpod, Pos);
        RpcSniperGun(sniperpod, whichpodchosed);
        CurrentSniperGuns++;
    }
    [Server]
    void SrvShutGun(Vector3 Pos)
    {
        var shutgunpod = GetShutGunPod();
        RpcSetPos(shutgunpod, Pos);
        RpcShutGun(shutgunpod, whichpodchosed);
        CurrentShutGuns++;
    }
    [Server]
    void SrvRPG(Vector3 Pos)
    {
        var rpgpod = GetRPGPod();
        RpcSetPos(rpgpod, Pos);
        RpcRPG(rpgpod, whichpodchosed);
        CurrentRPGs++;
    }



    // Spawn Position without Obstacles 
    public Vector3 SetPodPos()
    {
        Vector3 Position = new Vector3();
        bool ValidPosition = false;
        int randomednum;
        while (!ValidPosition)
        {
            randomednum = Random.Range(0, PodPositions.Length);
            if (!PodPositions[randomednum].isfull)
            {
                PodPositions[randomednum].isfull = true;
                Position = PodPositions[randomednum].transform.position;
                ValidPosition = true;
                WhichPod(randomednum);
                return Position;
            }
        }
        /*while (!ValidPosition)
        {
            Position = Center + new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 0, Random.Range(-Size.z / 2, Size.z / 2));
            Collider[] Colliders = Physics.OverlapSphere(Position, ObstacleRadius);
            ValidPosition = true;
            foreach (var col in Colliders)
            {
                //print(col.gameObject.name);
                if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Pod" || col.gameObject.tag == "Coin") ValidPosition = false;
            }
        }*/
        return Position;
    }

    void WhichPod(int Num)
    {
        whichpodchosed = Num;
    }
    
    public void MakePodAvailable(int num)
    {
        PodPositions[num].isfull = false;
    }

    public GameObject GetSimplePod()
    {
        for (int i = 0; i < SimpleGunPodList.Count; i++)
        {
            if (!SimpleGunPodList[i].activeInHierarchy)
            {
                return SimpleGunPodList[i];
            }
        }
        if (SimpleGunPodList.Count < AvailableSimpleGuns)
        {
            GameObject go = (GameObject)Instantiate(SimpleGunPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            SimpleGunPodList.Add(go);
            //Debug.Log(SimpleGunPodList.Count);
            return go;
        }

        return null;
    }

    public GameObject GetSecondGunPod()
    {
        for(int i=0; i < SecondGunPodList.Count; i++)
        {
            if (!SecondGunPodList[i].activeInHierarchy)
            {
                return SecondGunPodList[i];
            }
        }
        if(SecondGunPodList.Count < AvailableSecondGuns)
        {
            GameObject go = (GameObject)Instantiate(SecondGunPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            SecondGunPodList.Add(go);
            return go;
        }
        return null;
    }

    public GameObject GetRifleGunPod()
    {
        for (int i = 0; i < RifleGunPodList.Count; i++)
        {
            if (!RifleGunPodList[i].activeInHierarchy)
            {
                return RifleGunPodList[i];
            }
        }
        if (RifleGunPodList.Count < AvailableRifleGuns)
        {
            GameObject go = (GameObject)Instantiate(RifleGunPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            RifleGunPodList.Add(go);
            return go;
        }
        return null;
    }


    public GameObject GetSniperGunPod()
    {
        for (int i = 0; i < SniperGunPodList.Count; i++)
        {
            if (!SniperGunPodList[i].activeInHierarchy)
            {
                return SniperGunPodList[i];
            }
        }
        if (SniperGunPodList.Count < AvailableRifleGuns)
        {
            GameObject go = (GameObject)Instantiate(SniperGunPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            SniperGunPodList.Add(go);
            return go;
        }
        return null;
    }

    public GameObject GetShutGunPod()
    {
        for (int i = 0; i < ShutGunPodList.Count; i++)
        {
            if (!ShutGunPodList[i].activeInHierarchy)
            {
                return ShutGunPodList[i];
            }
        }
        if (ShutGunPodList.Count < AvailableShutGuns)
        {
            print("Spawned");
            GameObject go = (GameObject)Instantiate(ShutGunPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            ShutGunPodList.Add(go);
            return go;
        }
        return null;
    }

    public GameObject GetRPGPod()
    {
        for (int i = 0; i < RPGPodList.Count; i++)
        {
            if (!RPGPodList[i].activeInHierarchy)
            {
                return RPGPodList[i];
            }
        }
        if (RPGPodList.Count < AvailableRPGs)
        {
            GameObject go = (GameObject)Instantiate(RPGPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            RPGPodList.Add(go);
            return go;
        }
        return null;
    }


    public GameObject GetShieldPod()
    {
        for(int i=0; i < ShieldPodList.Count; i++)
        {
            if (!ShieldPodList[i].activeInHierarchy)
            {
                return ShieldPodList[i];
            }
        }
        if(ShieldPodList.Count < AvailableShields)
        {
            GameObject go = (GameObject)Instantiate(ShieldPod);
            go.SetActive(false);
            if (isServer) NetworkServer.Spawn(go);
            //else CmdNetworkSpawn(go);
            ShieldPodList.Add(go);
            return go;
        }
        return null;
    }


}
