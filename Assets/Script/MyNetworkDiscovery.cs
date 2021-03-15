using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class MyNetworkDiscovery : NetworkDiscovery { 
    bool Recieved;
    NetworkLobbyManager LobbyManager;
    private void Start()
    {
        LobbyManager = GetComponent<NetworkLobbyManager>();
    }
    LobbyMainMenu mainMenu;
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //print("Recieved");
        mainMenu = GetComponentInChildren<LobbyMainMenu>();
        if (mainMenu == null) return;
        string FakeAddress = fromAddress;
        string OriginalAddress = "";
        for(int i = 0; i < FakeAddress.Length; i++)
        {
            
            if (char.IsNumber(FakeAddress[i]) || FakeAddress[i] == '.')
            {
                OriginalAddress = OriginalAddress + FakeAddress[i];
            }
        }
        mainMenu.ipInput.text = OriginalAddress;
        //_mainMenu.CreateClient(fromAddress);
        mainMenu.AutoJoinButton.GetComponentInChildren<Text>().text = "Find IP";
        mainMenu.AutoJoinButton.interactable = true;
        Recieved = true;
    }
    private void LateUpdate()
    {
        if (Recieved) {
            StopBroadcast();
            Recieved = false;
        }
    }
}
