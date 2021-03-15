using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetPlayerName : NetworkBehaviour {
    [SyncVar]
    public string PlayerName;
    public Text PlayerNameText;
    [SyncVar]
    public int PlayerID;
	// Use this for initialization
	void Start () {
        PlayerNameText.text = PlayerName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /*void OnNameChange(string Pl)
    {
        Pl = PlayerName;
        PlayerNameText.text = Pl;
    }*/
}
