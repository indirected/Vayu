using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraSet : NetworkBehaviour {
    /*public Vector3 DistanceFromGameObject;
    public Vector3 RotationOfCamera;*/

    public GameObject CameraPrefab;
    public Transform CameraStartPosition;
    public Vector3 RotationOfCamera;
    public Vector3 DistanceFromGameObject;
    private GameObject PlayerCamera;

    // Use this for initialization
    void Start () {
        
    }
    public override void OnStartLocalPlayer()
    {
        PlayerCamera = Instantiate(CameraPrefab, CameraStartPosition.position, Quaternion.identity) as GameObject;
        PlayerCamera.transform.Rotate(RotationOfCamera);
    }
    // Update is called once per frame
    Vector3 CamPos;
    void FixedUpdate () {
        if (!isLocalPlayer) return;
        CamPos = new Vector3();
        CamPos.x = transform.position.x + DistanceFromGameObject.x;
        CamPos.y = transform.position.y + DistanceFromGameObject.y;
        CamPos.z = transform.position.z + DistanceFromGameObject.z;
        PlayerCamera.transform.position = CamPos;
    }
    [Command]
    void CmdSpawnCamera()
    {
        NetworkServer.Spawn(PlayerCamera);
    }
}
