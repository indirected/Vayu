using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class CoinPickupSystem : NetworkBehaviour {
    public float LevelUpValue;
    public GameObject[] Osbtacles;
    public float ObstacleRadius;
    Level LevelScript;
    Transform other;
    private void OnTriggerEnter(Collider collision)
    {
        try
        {
            LevelScript = collision.gameObject.GetComponentInParent<Level>();
            other = collision.gameObject.transform.parent.transform.parent;
            if (other.tag == "Player" && collision.GetComponentInParent<Level>().isLocalPlayer)
            {
                LevelScript.UpdateLvl(LevelUpValue);
                ChangeCoinPosition();
            }
        }
        catch
        {
        }        
    }
    CoinSystemControll CoinScript;
    public void ChangeCoinPosition()
    {
        CoinScript = FindObjectOfType<CoinSystemControll>();
        if (CoinScript.isServer) CoinScript.RpcDestroyCoin(gameObject, FindObjectOfType<RandomCoinSpawn>().SetCoinPos());
        else CoinScript.CmdDestroyCoin(gameObject, FindObjectOfType<RandomCoinSpawn>().SetCoinPos());
    }

}
