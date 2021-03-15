using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class RandomCoinSpawn : NetworkBehaviour {
    public Vector3 Center = new Vector3();
    public Vector3 Size = new Vector3();
    public Transform CoinsParent;
    public GameObject CoinObject;
    [HideInInspector]
    public List<GameObject> Coins;
    public int CoinsPerCar;
    public int MaxCoins;
    GameObject[] Players;
    CoinSystemControll Script;
    public float ObstacleRadius;
    private void Start()
    {
        //Script = FindObjectOfType<CoinSystemControll>();
        //Players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(StartCoins());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Center, Size);
    }
    public Vector3 SetCoinPos()
    {
        Vector3 Position = new Vector3();
        bool ValidPosition = false;
        var CoinScript = FindObjectOfType<CoinSystemControll>();
        while (!ValidPosition)
        {
            Position = Center + new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 0, Random.Range(-Size.z / 2, Size.z / 2));
            Collider[] Colliders = Physics.OverlapSphere(Position, ObstacleRadius);
            ValidPosition = true;
            foreach (var col in Colliders)
            {
                //print(col.gameObject.name);
                if (col.gameObject.tag == "Obstacle") ValidPosition = false;
            }
        }
        if (ValidPosition)
            return Position;
        else return new Vector3();
    }
    public void CreateCoins(int Value)
    {
        while (Value > 0)
        {
            GameObject NewCoin = Instantiate(CoinObject, SetCoinPos(), Quaternion.identity);
            Coins.Add(NewCoin);
            //NewCoin.SetActive(false);
            NewCoin.transform.parent = CoinsParent;
            Value--;
        }
    }
    public void DestroyCoins(int Value)
    {
        while (Value > 0)
        {
            var Coin = GameObject.FindWithTag("Coin");
            Destroy(Coin);
            Value--;
        }
    }
    IEnumerator StartCoins()
    {
        yield return new WaitForSecondsRealtime(1);
        Players = GameObject.FindGameObjectsWithTag("Player");
        //Coins Need To LevelUp
        //int CoinsNeed = Mathf.RoundToInt(Players[0].GetComponent<Level>().MaxLevelBarValue / CoinObject.GetComponent<CoinPickupSystem>().LevelUpValue);
        //Coins Value Must Create for Players
        var CoinsValue = CoinsPerCar * Players.Length ;
        //print(CoinsValue);
        yield return new WaitForSecondsRealtime(1);
        if(CoinsValue < MaxCoins)
            SrvCreateAndSpawnCoin(CoinObject, CoinsValue, SetCoinPos(), CoinsParent.gameObject);
        else SrvCreateAndSpawnCoin(CoinObject, MaxCoins, SetCoinPos(), CoinsParent.gameObject);
        yield return null;
    }
    public void OnPlayerDisconnected(NetworkPlayer player)
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        print(Players.Length);
        //Coins Need to levelUp
        int CoinsNeed = Mathf.RoundToInt(Players[0].GetComponent<Level>().MaxLevelBarValue / CoinObject.GetComponent<CoinPickupSystem>().LevelUpValue);
        var CoinsValue = CoinsNeed * Players.Length / 3;      
        if (CoinsNeed > CoinsValue)
            Script.CmdUnSpawnCoin(CoinObject , CoinsNeed - CoinsValue);
        
    }
    [Server]
    public void SrvCreateAndSpawnCoin(GameObject go, int Value, Vector3 Pos, GameObject Parent)
    {
        while (Value > 0)
        {
            GameObject NewCoin = Instantiate(go, SetCoinPos(), Quaternion.identity) as GameObject;
            NetworkServer.Spawn(NewCoin);
            Value--;
        }
    }
}
