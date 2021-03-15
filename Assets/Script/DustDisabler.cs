using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.Networking;
public class DustDisabler : NetworkBehaviour {

    public int StartEmissionSpeed = 3;
    private float speed;
    public ParticleSystem smoke1;
    public ParticleSystem smoke2;
    private ParticleSystem.EmissionModule em1, em2;
    private WheelCollider backleft;
    private WheelCollider backright;

	// Use this for initialization
	void Start () {
        em1 = smoke1.emission;
        em2 = smoke2.emission;
        backleft = GetComponent<CarController>().m_WheelColliders[3];
        backright = GetComponent<CarController>().m_WheelColliders[2];
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        speed = gameObject.GetComponent<CarController>().CurrentSpeed;
        if ((-StartEmissionSpeed < speed && speed < StartEmissionSpeed) || (!backleft.isGrounded && !backright.isGrounded) ) CmdDis();
        else CmdEn();
    }

    [Command]
    void CmdDis()
    {
        RpcDis();
    }
    [Command]
    void CmdEn()
    {
        RpcEn();
    }

    [ClientRpc]
    void RpcDis()
    {
        em1.enabled = false;
        em2.enabled = false;
    }

    [ClientRpc]
    void RpcEn()
    {
        em1.enabled = true;
        em2.enabled = true;

    }
}
