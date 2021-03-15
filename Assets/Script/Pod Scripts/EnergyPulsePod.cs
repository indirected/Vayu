using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPulsePod : MonoBehaviour {
    GameObject Player;
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Player = other.transform.parent.parent.gameObject;
            if(Player.tag == "Player")
            {
                Player.GetComponent<EnergyPulse>().HaveEnergy = true;
            }
        }
        catch { }
    }
}
