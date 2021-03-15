using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using UnityEditor;
using UnityStandardAssets.Vehicles.Car;
using System;

// Car Options For Special Class
[Serializable]
public class CarOptions
{
    [Header("-Health Options")]
    public int MaxHealth;
    public int MaxShield;
    [Header("-Moving Options And Physics")]   
    public GameObject[] WheelMeshes;
    public float Mass;
    public float TopSpeed;
    public int AutoBrakeTorque;
    public int BrakeTorque;
    public int Accelration;
    [Header("-Wepoan Options")]
    public bool DifferentWepoanOptions;
    public GameObject SimpleGun;
    public Transform SGShotPos1;
    public Transform SGShotPos2;
    public float SGBulletSpeed;
    public float SGReloadRate;
    public int SGMagzine;
    public GameObject SecondGun;
    public Transform SecondGunShotPoint1;
    public Transform SecondGunShotPoint2;
    public float SecondGBulletSpeed;
    public float SecondGReloadRate;
    public int SecondGMagzine;
    [Header("-Colliders Options")]
    public Vector3 ColliderCenter;
    public Vector3 ColliderSize;
    public float WheelsRadius;
    public Vector3 FrontRightWheelPos;
    public Vector3 FrontLeftWheelPos;
    public Vector3 RearRightWheelPos;
    public Vector3 RearLeftWheelPos;
}


public class SetCarClasses : NetworkBehaviour {
    [SyncVar]
    public LobbyPlayer.Car_Type m_Car_Type;
    //Cars Settings 
    //DPS Options
    //[Header("")]
    [SyncVar]
    public int Carid;
    public WheelCollider[] WheelColliders;
    public BoxCollider ButtomCollider;
    public CarOptions DPSCarOptions;
    public CarOptions EngineerCarOptions;
    public CarOptions TankCarOptions;
    //Car Models 
    public GameObject DPSCarPrefab;
    public GameObject EngineerCarPrefab;
    public GameObject TankCarPrefab;
	// Use this for initialization
	void Start () {
        StartCoroutine(SetCarClassCroutine());
    }
    // Enable Cars And Set Settings
    public void EnableDPSCar(GameObject Player)
    {
        // Applying DPS Car
        DPSCarPrefab.SetActive(true);
        EngineerCarPrefab.SetActive(false);
        TankCarPrefab.SetActive(false);
        // Health And Shield
        Health HPScript = Player.GetComponent<Health>();
        HPScript.maxHealth = DPSCarOptions.MaxHealth;
        Shield ShScript = Player.GetComponent<Shield>();
        ShScript.MaxShieldValue = DPSCarOptions.MaxShield;
        ShScript.ShieldBar.maxValue = DPSCarOptions.MaxShield;
        CarController CarScript = Player.GetComponent<CarController>();
        for(int i=0;i < 4; i++)
        {
            WheelColliders[i].radius = DPSCarOptions.WheelsRadius;
        }
        //Wheel Settings 
        WheelColliders[0].transform.position = DPSCarPrefab.transform.position + DPSCarOptions.FrontRightWheelPos;
        WheelColliders[1].transform.position = DPSCarPrefab.transform.position + DPSCarOptions.FrontLeftWheelPos;
        WheelColliders[2].transform.position = DPSCarPrefab.transform.position + DPSCarOptions.RearRightWheelPos;
        WheelColliders[3].transform.position = DPSCarPrefab.transform.position + DPSCarOptions.RearLeftWheelPos;
        ButtomCollider.center = DPSCarOptions.ColliderCenter;
        ButtomCollider.size = DPSCarOptions.ColliderSize;

        Player.GetComponent<Rigidbody>().mass = DPSCarOptions.Mass;
        CarScript.m_Topspeed = DPSCarOptions.TopSpeed;
        CarScript.AutoBrakeTorque = DPSCarOptions.AutoBrakeTorque;
        CarScript.BrakeTorque = DPSCarOptions.BrakeTorque;
        Player.GetComponent<CarUserControlMouse>().Accelaration = DPSCarOptions.Accelration;
        WeaponPickingSystem GunScript = Player.GetComponent<WeaponPickingSystem>();
        GunScript.SimpleGun = DPSCarOptions.SimpleGun;
        GunScript.SGShotPos1 = DPSCarOptions.SGShotPos1;
        GunScript.SGShotPos2 = DPSCarOptions.SGShotPos2;
        GunScript.SGMagzine = DPSCarOptions.SGMagzine;
        GunScript.SGReloadRate = DPSCarOptions.SGReloadRate;
        GunScript.SGBulletSpeed = DPSCarOptions.SGBulletSpeed;
        GunScript.SecondGun = DPSCarOptions.SecondGun;
        GunScript.SecondGunShotPoint1 = DPSCarOptions.SecondGunShotPoint1;
        GunScript.SecondGunShotPoint2 = DPSCarOptions.SecondGunShotPoint2;
        GunScript.SecondGMagzine = DPSCarOptions.SecondGMagzine;
        GunScript.SecondGReloadRate = DPSCarOptions.SecondGReloadRate;
        GunScript.SecondGBulletSpeed = DPSCarOptions.SecondGBulletSpeed;
        //CarScript.enabled = true;
    }
    public void EnableEngineerCar(GameObject Player)
    {
        // Applying DPS Car
        DPSCarPrefab.SetActive(false);
        EngineerCarPrefab.SetActive(true);
        TankCarPrefab.SetActive(false);
        // Health And Shield
        Health HPScript = Player.GetComponent<Health>();
        HPScript.maxHealth = EngineerCarOptions.MaxHealth;
        Shield ShScript = Player.GetComponent<Shield>();
        ShScript.MaxShieldValue = EngineerCarOptions.MaxShield;
        ShScript.ShieldBar.maxValue = EngineerCarOptions.MaxShield;
        CarController CarScript = GetComponent<CarController>();
        for (int i = 0; i < 4; i++)
        {
            WheelColliders[i].radius = EngineerCarOptions.WheelsRadius;
        }
        //Wheel Settings 
        WheelColliders[0].transform.position = EngineerCarPrefab.transform.position + EngineerCarOptions.FrontRightWheelPos;
        WheelColliders[1].transform.position = EngineerCarPrefab.transform.position + EngineerCarOptions.FrontLeftWheelPos;
        WheelColliders[2].transform.position = EngineerCarPrefab.transform.position + EngineerCarOptions.RearRightWheelPos;
        WheelColliders[3].transform.position = EngineerCarPrefab.transform.position + EngineerCarOptions.RearLeftWheelPos;
        ButtomCollider.center = EngineerCarOptions.ColliderCenter;
        ButtomCollider.size = EngineerCarOptions.ColliderSize;


        CarScript.m_Rigidbody.mass = EngineerCarOptions.Mass;
        CarScript.m_Topspeed = EngineerCarOptions.TopSpeed;
        CarScript.AutoBrakeTorque = EngineerCarOptions.AutoBrakeTorque;
        CarScript.BrakeTorque = EngineerCarOptions.BrakeTorque;
        GetComponent<CarUserControl>().Accelaration = EngineerCarOptions.Accelration;
        WeaponPickingSystem GunScript = Player.GetComponent<WeaponPickingSystem>();
        GunScript.SimpleGun = EngineerCarOptions.SimpleGun;
        GunScript.SGShotPos1 = EngineerCarOptions.SGShotPos1;
        GunScript.SGShotPos2 = EngineerCarOptions.SGShotPos2;
        GunScript.SGMagzine = EngineerCarOptions.SGMagzine;
        GunScript.SGReloadRate = EngineerCarOptions.SGReloadRate;
        GunScript.SGBulletSpeed = EngineerCarOptions.SGBulletSpeed;
        GunScript.SecondGun = EngineerCarOptions.SecondGun;
        GunScript.SecondGunShotPoint1 = EngineerCarOptions.SecondGunShotPoint1;
        GunScript.SecondGunShotPoint2 = EngineerCarOptions.SecondGunShotPoint2;
        GunScript.SecondGMagzine = EngineerCarOptions.SecondGMagzine;
        GunScript.SecondGReloadRate = EngineerCarOptions.SecondGReloadRate;
        GunScript.SecondGBulletSpeed = EngineerCarOptions.SecondGBulletSpeed;
    }
    public void EnableTankCar(GameObject Player)
    {
        // Applying DPS Car
        DPSCarPrefab.SetActive(false);
        EngineerCarPrefab.SetActive(false);
        TankCarPrefab.SetActive(true);
        // Health And Shield
        Health HPScript = Player.GetComponent<Health>();
        HPScript.maxHealth = TankCarOptions.MaxHealth;
        Shield ShScript = Player.GetComponent<Shield>();
        ShScript.MaxShieldValue = TankCarOptions.MaxShield;
        ShScript.ShieldBar.maxValue = TankCarOptions.MaxShield;
        CarController CarScript = Player.GetComponent<CarController>();
        for (int i = 0; i < 4; i++)
        {
            WheelColliders[i].radius = TankCarOptions.WheelsRadius;
        }
        //Wheel Settings 
        WheelColliders[0].transform.position = TankCarPrefab.transform.position + TankCarOptions.FrontRightWheelPos;
        WheelColliders[1].transform.position = TankCarPrefab.transform.position + TankCarOptions.FrontLeftWheelPos;
        WheelColliders[2].transform.position = TankCarPrefab.transform.position + TankCarOptions.RearRightWheelPos;
        WheelColliders[3].transform.position = TankCarPrefab.transform.position + TankCarOptions.RearLeftWheelPos;
        ButtomCollider.center = TankCarOptions.ColliderCenter;
        ButtomCollider.size = TankCarOptions.ColliderSize;


        CarScript.m_Rigidbody.mass = TankCarOptions.Mass;
        CarScript.m_Topspeed = TankCarOptions.TopSpeed;
        CarScript.AutoBrakeTorque = TankCarOptions.AutoBrakeTorque;
        CarScript.BrakeTorque = TankCarOptions.BrakeTorque;
        
        // Set Wepoan Options 

        Player.GetComponent<CarUserControl>().Accelaration = TankCarOptions.Accelration;
        WeaponPickingSystem GunScript = Player.GetComponent<WeaponPickingSystem>();
        GunScript.SimpleGun = TankCarOptions.SimpleGun;
        GunScript.SGShotPos1 = TankCarOptions.SGShotPos1;
        GunScript.SGShotPos2 = TankCarOptions.SGShotPos2;
        GunScript.SGMagzine = TankCarOptions.SGMagzine;
        GunScript.SGReloadRate = TankCarOptions.SGReloadRate;
        GunScript.SGBulletSpeed = TankCarOptions.SGBulletSpeed;
        GunScript.SecondGun = TankCarOptions.SecondGun;
        GunScript.SecondGunShotPoint1 = TankCarOptions.SecondGunShotPoint1;
        GunScript.SecondGunShotPoint2 = TankCarOptions.SecondGunShotPoint2;
        GunScript.SecondGMagzine = TankCarOptions.SecondGMagzine;
        GunScript.SecondGReloadRate = TankCarOptions.SecondGReloadRate;
        GunScript.SecondGBulletSpeed = TankCarOptions.SecondGBulletSpeed;
    }
    [ClientRpc]
    public void RpcChangeClass(GameObject Player)
    {
        LobbyPlayer.Car_Type CarType = Player.GetComponent<SetCarClasses>().m_Car_Type;
        if (CarType == LobbyPlayer.Car_Type.DPS) EnableDPSCar(Player);
        else if (CarType == LobbyPlayer.Car_Type.Engineer) EnableEngineerCar(Player);
        else EnableTankCar(Player);
    }
    [Server]
    public void SrvChangeClasses(GameObject Player)
    {
        //rs(Player);
    }
    /*[Server]
    public void SrvChangeClass(GameObject Player , GameObject CarClass)
    {
        //Player prefab is a global variable that refers to a prefab.


        //This line spawns the prefab just like any signle player game on the server.
        GameObject go = Instantiate(CarClass, transform.position, Player.transform.rotation);
        go.AddComponent(typeof(LookAtMouse));
        //Then this just tells all the clients to spawn the same object.
        NetworkServer.Spawn(go);
        //Then we try to replace it. And if it succeeded.
        if (NetworkServer.ReplacePlayerForConnection(connectionToClient, go, playerControllerId))
        {
            //We destroy the current player across all instances
            NetworkServer.Destroy(Player);
            RpcGiveLook(go);
        }
    }*/
    [ClientRpc]
    public  void RpcGiveLook(GameObject Player)
    {
        Player.AddComponent(typeof(LookAtMouse));
    }
    [Command]
    public void CmdChangeClass(GameObject Player)
    {
        LobbyPlayer.Car_Type Type = Player.GetComponent<SetCarClasses>().m_Car_Type;
        if (Type == LobbyPlayer.Car_Type.DPS) EnableDPSCar(Player);
        else if (Type == LobbyPlayer.Car_Type.Engineer) EnableEngineerCar(Player);
        else EnableTankCar(Player);
        RpcChangeClass(Player);
    }
    public IEnumerator SetCarClassCroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        if (isServer) RpcChangeClass(gameObject);
        else CmdChangeClass(gameObject);

    }
}
