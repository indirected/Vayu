using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class NetworkCharacterSelector : NetworkBehaviour {

    public GameObject DPSCar;
    public GameObject EngineerCar;
    public GameObject TankCar;

    
    public LobbyPlayer.Car_Type SelectedCar;
    public string PlayerName;
    public int PlayerID;

	// Use this for initialization
	void Start () {
        Debug.Log(SelectedCar);
        Invoke("CmdCharacterSelection", 0.2f);
	}
	
    [Command]
    void CmdCharacterSelection()
    {
        if(SelectedCar == LobbyPlayer.Car_Type.DPS)
        {
            GameObject go = Instantiate(DPSCar, transform.position, Quaternion.identity);
            SetPlayerName Script = go.GetComponent<SetPlayerName>();
            Script.PlayerName = PlayerName;
            Script.PlayerID = PlayerID;
            NetworkServer.Spawn(go);
            if (NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
            {
                NetworkServer.Destroy(gameObject);
                DestroyNonPlayers();
            }
            //else CmdCharacterSelection();
        }
        else if (SelectedCar == LobbyPlayer.Car_Type.Engineer)
        {
            GameObject go = Instantiate(EngineerCar, transform.position, Quaternion.identity);
            SetPlayerName Script = go.GetComponent<SetPlayerName>();
            Script.PlayerName = PlayerName;
            Script.PlayerID = PlayerID;
            NetworkServer.Spawn(go);
            if (NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
            {
                NetworkServer.Destroy(gameObject);
                DestroyNonPlayers();

            }
            // else CmdCharacterSelection();
        }
        else if (SelectedCar == LobbyPlayer.Car_Type.Tank)
        {
            GameObject go = Instantiate(TankCar, transform.position, Quaternion.identity);
            SetPlayerName Script = go.GetComponent<SetPlayerName>();
            Script.PlayerName = PlayerName;
            Script.PlayerID = PlayerID;
            NetworkServer.Spawn(go);
            if (NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
            {
                NetworkServer.Destroy(gameObject);
                DestroyNonPlayers();

            }
            //else CmdCharacterSelection();
        }
    }


    void DestroyNonPlayers()
    {
        GameObject[] AllCars = GameObject.FindGameObjectsWithTag("Player");
        foreach(var item in AllCars)
        {
            if(item.GetComponent<SetPlayerName>().PlayerName == "")
            {
                Destroy(item);
            }
        }
    }
	
}
