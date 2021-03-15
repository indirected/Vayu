using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EngineerRepair : NetworkBehaviour {
    public KeyCode RepairCode;
    public int RepairValue;
    public int RepairDelayTime;
    [HideInInspector]
    public bool IsPossible = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(RepairCode)) Repair(RepairValue);
	}
    public void Repair(int Value)
    {
        if (!IsPossible) return;
        var HealthScript = GetComponent<Health>();
        var NewHealth = HealthScript.CurrentHealth + Value;
        if(NewHealth > HealthScript.maxHealth)
        {
            HealthScript.CurrentHealth = HealthScript.maxHealth;
        }
        else
        {
            HealthScript.CurrentHealth = NewHealth;
        }
        IsPossible = false;
       // StartCoroutine(RepaiDelay());
    }
    IEnumerator RepaiDelay()
    {
        yield return new WaitForSecondsRealtime(RepairDelayTime);
        IsPossible = true;
    }
}
