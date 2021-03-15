using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Shield : NetworkBehaviour {
    public Slider ShieldBar;
    [SyncVar(hook ="OnShieldChange")]
    public float ShieldValue;
    public float MaxShieldValue;
    [SyncVar]
    public bool IsShield;
    public GameObject[] AllPlayers;
    public GameObject ShieldObj;
    void Start()
    {
        StartCoroutine(SetShieldOptions());
    }
    //Enable Shield For Player And Send For All Clients
    public void EnableShield()
    {
        IsShield = true;        
        if (isServer) RpcEnableShield();
        else CmdEnableShield();
        ShieldBar.value = ShieldValue = MaxShieldValue;
       
    }
    // Disable Shield 
    public void DisableShield()
    {
        IsShield = false;
        if (isServer) RpcDisableShield();
        else CmdDisableShield();
        
    }
    // Hook Voids 
    void OnShieldChange(float shVal)
    {
        ShieldBar.value = shVal;
    }
    // Rpcs
    [ClientRpc]
    public void RpcEnableShield()
    {
        ShieldBar.gameObject.SetActive(true);
        ShieldObj.SetActive(true);
        //ShieldBar.enabled = true;
    }
    [ClientRpc]
    public void RpcDisableShield()
    {
        ShieldBar.gameObject.SetActive(false);
        ShieldObj.SetActive(false);
        //ShieldBar.enabled = false;
    }
    // Commands
    [Command]
    public void CmdEnableShield()
    {
        ShieldBar.gameObject.SetActive(true);
        ShieldObj.SetActive(true);
        //ShieldBar.enabled = true;
        RpcEnableShield();
    }
    [Command]
    public void CmdDisableShield()
    {
        ShieldBar.gameObject.SetActive(false);
        ShieldObj.SetActive(false);
        //ShieldBar.enabled = false;
        RpcDisableShield();
    }
    IEnumerator SetShieldOptions()
    {
        yield return new WaitForSecondsRealtime(1);
        ShieldBar.maxValue = MaxShieldValue;
        ShieldBar.gameObject.SetActive(false);
        //ShieldBar.enabled = false;
        AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var Player in AllPlayers)
        {
            Shield ShieldScript = Player.GetComponent<Shield>();
            print(ShieldScript.IsShield);
            if (ShieldScript.IsShield && !ShieldScript.isLocalPlayer)
            {
                ShieldScript.ShieldBar.gameObject.SetActive(true);
                ShieldScript.ShieldBar.enabled = true;
                ShieldScript.ShieldBar.value = ShieldScript.ShieldValue;
            }
        }
    }
}
