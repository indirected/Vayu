using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoinSystemControll : NetworkBehaviour {
    public GameObject Script;
    private void Start()
    {
        //Script = FindObjectOfType<RandomCoinSpawn>();
    }
    [Command]
    public void CmdCreateAndSpawnCoin(GameObject go , int Value ,Vector3 Pos ,GameObject Parent)
    {
        while (Value > 0)
        {
            GameObject NewCoin = Instantiate(go, Script.GetComponent<RandomCoinSpawn>().SetCoinPos(), Quaternion.identity) as GameObject;
            NetworkServer.Spawn(NewCoin);
            Value--;
        }
    }
    [Command]
    public void CmdUnSpawnCoin(GameObject go , int Value)
    {
        while (Value > 0)
        {
            GameObject SpawnedCoin = GameObject.FindGameObjectWithTag("Coin");
            print("Player Disconnected");
            NetworkServer.UnSpawn(SpawnedCoin);
            Value--;
        }
    }
    [ClientRpc]
    public void RpcSetParent()
    {
        print("Disconnected");
    }
    [Command]
    public void CmdSetParent()
    {
        print("Disconnected");
        RpcSetParent();
    }
    [ClientRpc]
    public void RpcDestroyCoin(GameObject go , Vector3 CoinPos)
    {
        go.SetActive(false);
        go.transform.position = CoinPos;
        go.SetActive(true);
    }
    [Command]
    public void CmdDestroyCoin(GameObject go , Vector3 CoinPos)
    {
        go.SetActive(false);
        go.transform.position = CoinPos;
        go.SetActive(true);
        RpcDestroyCoin(go , CoinPos);
    }
    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        if (isServer) RpcSetParent();
        else CmdSetParent();
    }
}
