using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;
using UnityStandardAssets.Vehicles.Car;

[NetworkSettings(sendInterval = 0.02f)]
public class RoundController : NetworkBehaviour {

    public Text TimeText;
    public int RoundTimeSec;
    public bool IsRoundEnded;
    [HideInInspector]
    public bool Canfire =true;
    [Header("End Panel Settings")]
    public GameObject EndPanel;
    public Text NamesText;
    public Text KillsText;
    public Text DiesText;
    public Button Rematch;
    public float DPS_CarTopSpeed;
    public int RematchDelay;
    [HideInInspector]
    public string TimerStr;
    [HideInInspector]
    public GameObject[] AllPlayers;


    //SyncVars
    [SyncVar]
    private string Minutes;
    [SyncVar]
    private string Secondes;
    //[SyncVar]
    [HideInInspector]
    public string KillsString;
    //[SyncVar]
    [HideInInspector]
    public string NamesString;
    //[SyncVar]
    [HideInInspector]
    public string DiesString;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(CountDown());
	}

    [ClientRpc]
    void RpcSendTime()
    {
        TimeText.text = Minutes + ":" + Secondes;
    }
    [ClientRpc]
    void RpcSendTime1(string str)
    {
        GameObject.Find("Timer").GetComponent<Text>().text = "Match Will Starts in : " + str;
    }
    [ClientRpc]
    void RpcEmptyTimer()
    {
        GameObject.Find("Timer").GetComponent<Text>().text = "";
        Rematch.interactable = true;
    }
    IEnumerator CountDown()
    {
        for(int i=RoundTimeSec; i >= 0; i--)
        {
            if (isServer)
            {
                Minutes = (i / 60).ToString();
                Secondes = (i % 60).ToString();
            }     
            TimeText.text = Minutes + ":" + Secondes;
            //RpcSendTime();
            yield return new WaitForSecondsRealtime(1);
            if ((i / 60) == 0 && (i % 60) == 0) yield return null;
        }
    }
    private void LateUpdate()
    {
        int MinutesNum;
        int SecondsNum;
        int.TryParse(Minutes, out MinutesNum);
        int.TryParse(Secondes, out SecondsNum);
        if (MinutesNum == 0 && SecondsNum == 0 && !IsRoundEnded)
        {
            IsRoundEnded = true; 
            
            EndGame();
        }
    }
    [Server]
    public void EndGame()
    {
        RpcEnableEndPanel();
    }
    [ClientRpc]
    public void RpcEnableEndPanel()
    {
        FindObjectOfType<RoundController>().GetComponent<AudioSource>().Stop();
        Canfire = false;
        //Set Kills Feed Strings 
        NamesString = "Players" + "\n" + "\n";
        KillsString = "Kills" + "\n" + "\n";
        DiesString = "Dies" + "\n" + "\n";
        AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in AllPlayers)
        {
            string Name = item.GetComponent<SetPlayerName>().PlayerName;
            string Kill = item.GetComponent<KillsFeed>().KillValue.ToString();
            string Die = item.GetComponent<KillsFeed>().DiesValue.ToString();
            //print(Name);
            NamesString += Name + "\n" + "\n";
            KillsString += "  " + Kill + "\n" + "\n";
            DiesString += "  " + Die + "\n" + "\n";
        }
        //Stop Car Movments
        for(int i = 0; i < AllPlayers.Length; i++) 
        {
            AllPlayers[i].GetComponent<CarController>().m_Topspeed = 0;
        } 
        //Disable Game Objects And Players
        foreach(var item in AllPlayers)
        {
            var Meshes = item.GetComponentsInChildren<Renderer>();
            item.GetComponentInChildren<Canvas>().enabled = false;
            foreach (var mesh in Meshes)
            {
                mesh.enabled = false;
            }
            
        }
        Transform Canvas = GameObject.Find("Canvas1").transform;
        for(int i=0; i < Canvas.childCount; i++)
        {
            Canvas.GetChild(i).gameObject.SetActive(false);
        }
        //Enable End Panel
        EndPanel.SetActive(true);
        for (int i = 0; i < EndPanel.transform.childCount; i++)
        {
            EndPanel.transform.GetChild(i).gameObject.SetActive(true);
        }
        if (!isServer) EndPanel.transform.Find("Rematch").gameObject.SetActive(false);
        // Set Kills Feed Texts
        NamesText.text = NamesString;
        KillsText.text = KillsString;
        DiesText.text = DiesString;
        Time.timeScale = 0;
    }
    [ClientRpc]
    public void RpcResetOptions()
    {
        foreach (var item in AllPlayers) {
            KillsFeed Feed = item.GetComponent<KillsFeed>();
            Health Hp = item.GetComponent<Health>();
            Level Lvl = item.GetComponent<Level>();
            Shield shield = item.GetComponent<Shield>();
            WeaponPickingSystem weapon = item.GetComponent<WeaponPickingSystem>();
            if (shield.IsShield) shield.DisableShield();
            Feed.KillValue = 0;
            Feed.DiesValue = 0;
            Hp.CurrentHealth = Hp.maxHealth;
            Lvl.LevelBar.value = 0;
            Lvl.PlayerLevel = 1;
            Lvl.PlayerLevelTxt.text = "lvl." + Lvl.PlayerLevel;
            if (weapon.WhichGunisOn == WeaponPickingSystem.GUN_TYPE.secondGun)
            {
                weapon.SecondaryGunDiactive = true;
                weapon.SecondGun.SetActive(false);
                weapon.SecondaryGImg.enabled = false;
               // weapon.SecondaryGunText.color = Color.black;
                weapon.SecondarySlotOn = false;
                weapon.ActivateSimpleGun();
            }
        }
        IsRoundEnded = false;

    }
    [ClientRpc]
    public void RpcSpawn(GameObject go , Vector3 Pos)
    {
        go.GetComponentInChildren<Canvas>().enabled = true;
        var meshes = go.GetComponentsInChildren<Renderer>();
        go.transform.position = Pos;
        IsRoundEnded = false;
        foreach (var item in meshes)
        {
            item.enabled = true;
        }
        StartCoroutine(LimitSpeed(go));
        StartCoroutine(CountDown());
        Transform Canvas = GameObject.Find("Canvas1").transform;
        for (int i = 0; i < Canvas.childCount; i++)
        {
            Canvas.GetChild(i).gameObject.SetActive(true);
        }
        FindObjectOfType<RoundController>().GetComponent<AudioSource>().Play();
        EndPanel.SetActive(false);
        Time.timeScale = 1;
        Canfire = true;
    }
    [Server]
    public void ServerResetOptions()
    {
        foreach (var item in AllPlayers)
        {
            KillsFeed Feed = item.GetComponent<KillsFeed>();
            Health Hp = item.GetComponent<Health>();
            Level Lvl = item.GetComponent<Level>();
            Shield shield = item.GetComponent<Shield>();
            WeaponPickingSystem weapon = item.GetComponent<WeaponPickingSystem>();
            if (shield.IsShield) shield.DisableShield();
            Feed.KillValue = 0;
            Feed.DiesValue = 0;
            Hp.CurrentHealth = Hp.maxHealth;
            Lvl.LevelBar.value = 0;
            Lvl.PlayerLevel = 1;
            Lvl.PlayerLevelTxt.text = "lvl." + Lvl.PlayerLevel;
            if (weapon.WhichGunisOn == WeaponPickingSystem.GUN_TYPE.secondGun)
            {
                weapon.SecondaryGunDiactive = true;
                weapon.SecondGun.SetActive(false);
                weapon.SecondaryGImg.enabled = false;
                //weapon.SecondaryGunText.color = Color.black;
                weapon.SecondarySlotOn = false;
                weapon.ActivateSimpleGun();
            }
            else weapon.ActivateSimpleGun();
        }
        RpcResetOptions();
        
    }
    [Server]
    public void ServerStartRematch()
    {
        StartCoroutine(RematchDelayEnum());
    }
    public void OnClickExitButton()
    {
        var Lobby = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
        Lobby.GoBackButton(); 
    }
    public void OnClickRematch()
    {
        Rematch.interactable = false;
        ServerStartRematch();       
    }
    IEnumerator RematchDelayEnum()
    {
        for(int i = RematchDelay; i > 0; i--)
        {
            TimerStr = i.ToString();
            RpcSendTime1(TimerStr);
            yield return new WaitForSecondsRealtime(1);           
        }
        ServerResetOptions();
        ServerSpawn();
    }
    [Server]
    void ServerSpawn()
    {
        var StartPos = FindObjectsOfType<NetworkStartPosition>();
        int index = 0;
        RpcEmptyTimer();
        foreach(var Player in AllPlayers)
        {
            
            RpcSpawn(Player, StartPos[index].transform.position);
            index++;
            if (index >= StartPos.Length) index = 0;
        }
    }
    IEnumerator LimitSpeed(GameObject go)
    {
        CarController script = go.GetComponent<CarController>();
        script.m_Topspeed = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        script.m_Topspeed = DPS_CarTopSpeed;
    }
}
