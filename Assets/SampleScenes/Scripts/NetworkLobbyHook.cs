using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NetworkCharacterSelector PlayerScript = gamePlayer.GetComponent<NetworkCharacterSelector>();
        PlayerScript.PlayerName = lobby.playerName.ToString();
        PlayerScript.PlayerID = lobby.PlayerID;
        //gamePlayer.GetComponent<SetCarClasses>().m_Car_Type = lobby._CarType;
        gamePlayer.GetComponent<NetworkCharacterSelector>().SelectedCar = lobby._CarType;

    }
}
