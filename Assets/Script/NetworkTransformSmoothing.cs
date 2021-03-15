using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.001f)]

public class NetworkTransformSmoothing : NetworkBehaviour {

    public float UpdateRate = 0.01f;
    public float PositionSmoothRate = 4;
    public float RotationSmoothRate = 4;

    private Transform myTransform;
    private Vector3 PlayerPosition;
    private Quaternion PlayerRotation;

	// Use this for initialization
	void Start () {
        myTransform = transform;
        if (isLocalPlayer)
        {
            StartCoroutine(UpdateTransform());
        }
	}
	
    IEnumerator UpdateTransform()
    {
        while (enabled)
        {
            CmdSendTransform(myTransform.position, myTransform.rotation);
            yield return new WaitForSeconds(UpdateRate);
        }
    }

    void LerpTransform()
    {
        if (isLocalPlayer) return;
        
        myTransform.position = Vector3.Lerp(myTransform.position, PlayerPosition, Time.deltaTime * PositionSmoothRate);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, PlayerRotation, Time.deltaTime * RotationSmoothRate);
        Debug.Log(Time.deltaTime * PositionSmoothRate);
    }


    [Command]
    void CmdSendTransform(Vector3 Position, Quaternion Rotation)
    {
        PlayerPosition = Position;
        PlayerRotation = Rotation;
        RpcReciveTransform(Position,Rotation);
    }

    [ClientRpc]
    void RpcReciveTransform(Vector3 Position, Quaternion Rotation)
    {
        PlayerPosition = Position;
        PlayerRotation = Rotation;
    }

	// Update is called once per frame
	void Update () {
        LerpTransform();
	}
}
