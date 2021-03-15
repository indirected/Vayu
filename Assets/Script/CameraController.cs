using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

public class CameraController : NetworkBehaviour {
    public GameObject PlayerPrefab;
    public Vector3 DistanceFromGameObject;
    public Vector3 RotationOfCamera;
	// Use this for initialization
	void Start () {
        transform.Rotate(RotationOfCamera);

    }
    public override void OnStartLocalPlayer()
    {
        
    }
    
        
    // Update is called once per frame
    void FixedUpdate () {       
                            
    }
    public void SetCamera()
    {
        var CamPos = new Vector3();
        CamPos.x = PlayerPrefab.transform.position.x + DistanceFromGameObject.x;
        CamPos.y = PlayerPrefab.transform.position.y + DistanceFromGameObject.y;
        CamPos.z = PlayerPrefab.transform.position.z + DistanceFromGameObject.z;
        transform.position = CamPos;
    }
}
