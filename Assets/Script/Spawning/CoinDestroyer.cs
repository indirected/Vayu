using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoinDestroyer : NetworkLobbyManager {

	// Use this for initialization
	void Start () {
		
	}
	
	

    public void OnPlayerDisconnected(NetworkPlayer player)
    {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        var Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(Players.Length);
        //Coins Need to levelUp
        /*int CoinsNeed = Mathf.RoundToInt(Players[0].GetComponent<Level>().MaxLevelBarValue / CoinObject.GetComponent<CoinPickupSystem>().LevelUpValue);
        var CoinsValue = CoinsNeed * Players.Length / 3;
        if (CoinsNeed > CoinsValue)
            Script.CmdUnSpawnCoin(CoinObject, CoinsNeed - CoinsValue);*/

    }
}
