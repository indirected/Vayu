using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class RoundClassControll : NetworkBehaviour
{
    private void Start()
    {
        StartCoroutine(Setcars());
    }
    [Server]
    public void SrvSetClass()
    {
        SetCarClasses Script = FindObjectOfType<SetCarClasses>();
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(var item in AllPlayers)
        {
            
            Script.RpcChangeClass(item);
        }
    }
    
    IEnumerator Setcars()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        SrvSetClass();
    }
}
