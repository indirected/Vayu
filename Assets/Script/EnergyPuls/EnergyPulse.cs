using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnergyPulse : NetworkBehaviour {

    public KeyCode EnableEnergyKey;
    public bool HaveEnergy = true;
    public float Radius;
    public float Power;
    public float EnergyTime;
    public float ReloadTime;
    public Slider EnergySlider;
    private void Start()
    {
        EnergySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
        EnergySlider.maxValue = 1;
        HaveEnergy = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(EnableEnergyKey) && isLocalPlayer && HaveEnergy)
        {          
            StartCoroutine(StartEnergyPulse());
            HaveEnergy = false;
        }
    }




    public void EnableEnergyPulse()
    {
        
        Vector3 Center = new Vector3(transform.position.x + 0.1223469f, transform.position.y + (-1.144409e-05f) ,  transform.position.z + (-3.065914f));
        Collider[] NearbyColliders =  Physics.OverlapSphere(Center, Radius);
        print(NearbyColliders.Length);
        foreach(Collider item in NearbyColliders)
        {
            print(item.gameObject.name);
            if (item.gameObject.tag == "PlayerCollider" && !item.GetComponentInParent<NetworkIdentity>().isLocalPlayer)
            {
                var Player = item.transform.parent.transform.parent.gameObject;
                var Heading = Player.transform.position - transform.position;
                var distance = Heading.magnitude;
                var direction = Heading / distance;
                //Player.GetComponent<Rigidbody>().AddForce(direction, ForceMode.VelocityChange);
                if (isServer) RpcTranslatePlayer(Player , direction);
                else CmdTranslatePlayer(Player , direction);
                //Player.transform.Translate(Player.transform.forward * (-1) * Power);
            }
            print(item.gameObject.name);
        }
    }
    [ClientRpc]
    public void RpcTranslatePlayer(GameObject go , Vector3 Direction)
    {
        go.GetComponent<Rigidbody>().velocity = new Vector3();
        go.GetComponent<Rigidbody>().AddForce(Direction * Power, ForceMode.VelocityChange);
        //go.transform.Translate(Direction  , Space.World);
    }
    [Command]
    public void CmdTranslatePlayer(GameObject go , Vector3 Direction)
    {
        
        go.GetComponent<Rigidbody>().velocity = new Vector3();
        go.GetComponent<Rigidbody>().AddForce(Direction * Power, ForceMode.VelocityChange);
        //go.transform.Translate(Direction, Space.World);
        RpcTranslatePlayer(go , Direction);
    }
    public IEnumerator StartEnergyPulse()
    {
        for(float i = EnergyTime; i >= 0; i = i - 0.01f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            EnergySlider.value = i / EnergyTime;
            EnableEnergyPulse();
        }
        StartCoroutine(ReloadEnergy());
    }
    public IEnumerator ReloadEnergy()
    {
        for (float i = 0; i <= ReloadTime; i = i + 0.01f)
        {
            EnergySlider.value = i / ReloadTime;
            yield return new WaitForSeconds(0.01f);
        }
        HaveEnergy = true;
    }
}
