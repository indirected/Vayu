using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    internal enum RespawnType
    {
        DontResetLevel
    }
    [SerializeField] private RespawnType m_RespawnType = RespawnType.DontResetLevel;
    [HideInInspector] public int KillerId;
    public int maxHealth;
    [SyncVar(hook = "OnHealthChange")]
    public float CurrentHealth;
    [SyncVar(hook ="OnTextFeedChanged")]
    public string KillFeed;
    public Slider HealthSlider;
    private bool isReset;
    // Use this for initialization
    void Start () {
        HealthSlider.value = maxHealth;
	}	
    public void TakeDamage(float HowMuch , int Killer)
    {
        //if (!isServer) return;
        Shield ShieldScript = GetComponent<Shield>();
        var NewHealth = CurrentHealth - HowMuch;
        if (ShieldScript.IsShield)
        {
            var NewShield = new float();
            NewShield = ShieldScript.ShieldValue - HowMuch;
            if (NewShield < 0)
            {
                ShieldScript.DisableShield();
                HowMuch = Mathf.Abs(NewShield);
                print(HowMuch);
            }
            else if(NewShield > 0)
            {
                ShieldScript.ShieldValue = NewShield;
                return;
            }
            else
            {
                ShieldScript.DisableShield();
                return;
            }
        }
        if(NewHealth <= 0)
        {           
            string KillerName = null ;
            string DeadName = null;
            var AllPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject Pl in AllPlayers)
            {
                var PlayerID = Pl.GetComponent<SetPlayerName>().PlayerID;
                if(PlayerID == Killer)
                {
                    KillerName = Pl.GetComponent<SetPlayerName>().PlayerName;
                    Level levelscript = Pl.GetComponent<Level>();
                    //if(levelscript.isLocalPlayer)
                    levelscript.RpcLevelUp(Killer, levelscript.LevelBar.maxValue * 0.8f);
                    //print("Level Uppeddd");
                    KillsFeed KillsScript = Pl.GetComponent<KillsFeed>();
                    KillsScript.UpdateKills();                   
                    //KillsScript.KillsText.text = "Kills : " + KillsScript.KillValue;                    
                }
            }
            //if (isLocalPlayer)
            
            DeadName = gameObject.GetComponent<SetPlayerName>().PlayerName;
            KillsFeed DiesScript = gameObject.GetComponent<KillsFeed>();
            DiesScript.UpdateDies();
            //DiesScript.DiesText.text = "Dies : " + DiesScript.DiesValue;
            Respawn(KillerName, DeadName);
            SetHealth(maxHealth);
        }
        else
        {
            SetHealth(NewHealth);            
        }
    }
    ShotProjectile ProjectileScript;
    Health HealthScript;
    GameObject other;
    void OnHealthChange (float UpdatedHealth)
    {
        SetHealth(UpdatedHealth);
        HealthSlider.value = UpdatedHealth;        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (FindObjectOfType<RoundController>().IsRoundEnded) return;
        var other = collision.gameObject;
        if (other.tag == "Bullet")
        {
            ProjectileScript = other.GetComponent<ShotProjectile>();
            HealthScript = gameObject.GetComponentInParent<Health>();
            KillerId = ProjectileScript.PlayerId;
            if (GetComponent<SetPlayerName>().PlayerID == KillerId) return;
            HealthScript.TakeDamage(ProjectileScript.DamageValue , KillerId);           
           // print("Hit: " + Damage);
            //print("Currenthealth= " + gameObject.GetComponent<Health>().CurrentHealth);
        }
    }

    [ClientRpc]
    void RpcRespawn(string Killer , string Dead)
    {
        var Colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach(var item in Colliders)
        {
            item.enabled = false;
        }
        var meshes = gameObject.GetComponentsInChildren<Renderer>();
        foreach(var i in meshes)
        {
            i.enabled = false;
        }
        var SpawnPoints = FindObjectsOfType<NetworkStartPosition>();
        var ChosenPoint = Random.Range(0, SpawnPoints.Length);
        while (!SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().IsRespawnable)
        {
            ChosenPoint = Random.Range(0, SpawnPoints.Length);
        }
        gameObject.transform.position = SpawnPoints[ChosenPoint].transform.position;
        SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().IsRespawnable = false;
        StartCoroutine(SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().ResetBool());
        foreach (var item in Colliders)
        {
            item.enabled = true;
        }
        foreach (var i in meshes)
        {
            i.enabled = true;
        }
        //var text = GameObject.Find("KillFeedText").GetComponent<Text>();
        KillFeed = KillFeed  + "Player " + Killer + " Killed " + Dead + System.Environment.NewLine;
        StartCoroutine(DestroyFirstLine(KillFeed));
    }
    [Command]
    void CmdRespawn(string KillerName , string DeadName)
    {
        var Colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (var item in Colliders)
        {
            item.enabled = false;
        }
        var meshes = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var i in meshes)
        {
            i.enabled = false;
        }
        var SpawnPoints = FindObjectsOfType<NetworkStartPosition>();
        var ChosenPoint = Random.Range(0, SpawnPoints.Length);
        while (!SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().IsRespawnable)
        {
            ChosenPoint = Random.Range(0, SpawnPoints.Length);
        }
        gameObject.transform.position = SpawnPoints[ChosenPoint].transform.position;
        SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().IsRespawnable = false;
        StartCoroutine(SpawnPoints[ChosenPoint].GetComponent<SpawnPositions>().ResetBool());
        foreach (var item in Colliders)
        {
            item.enabled = true;
        }
        foreach (var i in meshes)
        {
            i.enabled = true;
        }
        RpcRespawn(KillerName , DeadName);
    }
    // Respawn Types
    void Respawn(string KillerName , string DeadName)
    {
        switch (m_RespawnType)
        {
            case RespawnType.DontResetLevel:
                isReset = false;
                if (isServer) RpcRespawn( KillerName , DeadName);
                else CmdRespawn(KillerName ,DeadName);
                break;
        }
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
        KillFeed = Text;
        //if (isLocalPlayer) GameObject.Find("KillFeedText").GetComponent<Text>().text = Text;
        if (lines.Length != 0) StartCoroutine(DestroyFirstLine(Text));
    }
    public void OnTextFeedChanged(string Text)
    {
        KillFeed = Text;
        
        try
        {
            GameObject.Find("KillFeedText").GetComponent<Text>().text = Text;
        }
        catch { }
    }[Command]
    public void CmdIntPlusOne(int value)
    {
        value++;
    }
    [Server]
    void SetHealth(float Value)
    {
        CurrentHealth = Value;
    }
}
