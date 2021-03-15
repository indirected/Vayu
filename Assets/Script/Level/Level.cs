using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Level : NetworkBehaviour {
    //[SyncVar(hook = "ChangeLvl")]
    public int PlayerLevel;
    public int MaxLevelBarValue;
    //[SyncVar(hook = "ChangeLevl")]
    public float level;
    public Slider LevelBar;
    private float MaxLevelValue;
    public Text PlayerLevelTxt;
	// Use this for initialization
	void Start () {
        PlayerLevelTxt = GameObject.Find("LevelProgressBar").GetComponentInChildren<Text>();
        LevelBar = GameObject.Find("LevelProgressBar").GetComponentInChildren<Slider>();
        LevelBar.maxValue = MaxLevelBarValue;
        PlayerLevel = 1;
        PlayerLevelTxt.text = "lvl." + PlayerLevel;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void UpdateLvl(float value)
    {
        if (!isLocalPlayer) return;
        MaxLevelValue = LevelBar.maxValue;
        var NewVal = LevelBar.value + value;
        if(NewVal >= MaxLevelValue)
        {
            NewVal -= MaxLevelValue;
            LevelBar.value = NewVal;
            print("Level Up !!");
            PlayerLevel++;
            PlayerLevelTxt.text = "Lvl." + PlayerLevel;
        }
        else
        {
            LevelBar.value = NewVal;
        }
    }
    void ChangeLvl(int lvl)
    {
        if(isLocalPlayer)
            PlayerLevelTxt.text = "Lvl." + lvl;
    }
    void ChangeLevl(float value)
    {
        if (isLocalPlayer)
        {
            level = value;
            LevelBar.value = value;
        }
    
    }
    [ClientRpc]
    public void RpcLevelUp(int id , float Value)
    {
        var Playerid = GetComponent<SetPlayerName>().PlayerID;
        if(Playerid == id)
        {
            UpdateLvl(Value);
        }
    }
}
