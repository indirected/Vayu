using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KillsFeed: NetworkBehaviour {
    [HideInInspector] public Text KillsText;
    [HideInInspector] public Text DiesText;   
    [SyncVar(hook = "OnKillIntChanged")]
    public int KillValue;   
    [SyncVar(hook = "OnDieIntChanged")]
    public int DiesValue;
    //[HideInInspector] public Text KillsFeedText;
	// Use this for initialization
	void Start () {
        KillsText = GameObject.Find("KillsText").GetComponent<Text>();
        DiesText = GameObject.Find("DiesText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        KillsText.text = "Kills : " + KillValue;
        DiesText.text = "Dies : " + DiesValue;
	}
    void OnKillIntChanged(int UpdatedValue)
    {
        KillValue = UpdatedValue;
        if(isLocalPlayer) KillsText.text = "Kills : " + KillValue;
    }
    private void OnDieIntChanged(int UpdatedValue)
    {
        DiesValue = UpdatedValue;
        if(isLocalPlayer) DiesText.text = "Dies : " + DiesValue;
    }
    //[Server]
    public void UpdateKills()
    {
        KillValue++;
    }
    //[Server]
    public void UpdateDies()
    {
        DiesValue++;
    }
    
}
