using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNameList : NetworkBehaviour {
    public string[] PlayersName = new string[10];
    private void Start()
    {
        StartCoroutine(UpdateNames());
    }
    IEnumerator UpdateNames()
    {
        yield return new WaitForSecondsRealtime(1f);
        SrvAddList();
    }

    // Network Functions
    [ClientRpc]
    public void RpcAddList()
    {
        
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(var item in AllPlayers)
        {
            int ID = item.GetComponent<SetPlayerName>().PlayerID;
            PlayersName[ID-1] = item.GetComponent<SetPlayerName>().PlayerName;
        }      
    }
    [Command]
    public void CmdAddList(string Name)
    {
        
        RpcAddList();
    }
    [Server]
    public void SrvAddList()
    {
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in AllPlayers)
        {
            int ID = item.GetComponent<SetPlayerName>().PlayerID;
            PlayersName[ID - 1] = item.GetComponent<SetPlayerName>().PlayerName;
        }
       RpcAddList();
    }
    [ClientRpc]
    public void RpcRemovePlayerFromList(int id)
    {
        for(int i = id; i < PlayersName.Length; i++)
        {
            if (i == PlayersName.Length - 1)
                PlayersName[i] = "";
            else
                PlayersName[i] = PlayersName[i + 1];
        }
    }
    [Server]
    public void SrvRemovePlayerFromList(int id)
    {
        for (int i = id; i < PlayersName.Length; i++)
        {
            if (i == PlayersName.Length - 1)
                PlayersName[i] = "";
            else
                PlayersName[i] = PlayersName[i + 1];
        }
        RpcRemovePlayerFromList(id);
    }
    [Server]
    public void SrvEditLogsText(string PlayerName)
    {
        FindObjectOfType<Health>().KillFeed += PlayerName + " Disconnected From Server" + System.Environment.NewLine;
        StartCoroutine(DestroyFirstLine(FindObjectOfType<Health>().KillFeed));
    }
    IEnumerator DestroyFirstLine(string Text)
    {
        var lines = Text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
        //print(lines.Length);
        if (lines.Length == 0) yield return null;
        yield return new WaitForSecondsRealtime(5);
        Text = "";
        for (int i = 1; i < lines.Length; i++)
        {
            Text = Text + lines[i] + System.Environment.NewLine;
        }
        lines = Text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
        FindObjectOfType<Health>().KillFeed = Text;
        //if (isLocalPlayer) GameObject.Find("KillFeedText").GetComponent<Text>().text = Text;
        if (lines.Length != 0) StartCoroutine(DestroyFirstLine(Text));
    }
}
