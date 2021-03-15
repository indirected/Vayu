using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ShootControll : NetworkBehaviour {

    //private WeaponPickingSystem.GUN_TYPE CurrentGun;
    public KeyCode ShootKey;
    private GameObject PoolControllerObject;
    private RoundController RoundController;
    //public GameObject BulletObject1;
    public Transform ShotPosition1;
    public Transform ShotPosition2;
    public float BulletSpeed;
    public float ReloadRate;
    private float nextShotTime;  //used for checking reload
    private int SmgHowManyatonce;
    private int RpgHowmanyatOnce;

    //Sniper
    private RaycastHit hit;


    public int PrimarySlotMagzineLeft = 1;  //used in WeaponPickingSystem.cs for assigning magzine Number
    public int SecondarySlotMagzineLeft = 1; //used in WeaponPickingSystem.cs for assigning magzine Number
    public int CurrentMagzine;
    private GameObject MagzineLeftObject;
    public Text MagzineLeftText;

    // Sounds 
    [Header("Guns Audio")]
    public AudioSource SoundSource;
    public AudioClip SimpleGun;
    public AudioClip SecondGun;
    public AudioClip RifleGun;
    public AudioClip ShotGun;
    public AudioClip ShotGunReload;
    public AudioClip SniperGun;
    public AudioClip SniperGunReload;
    //public AudioClip RPGGunSound;
    // Use this for initialization
    void Start () {
        Invoke("RecognizePool", 0.5f);
        MagzineLeftObject = GameObject.Find("MagzineText");
        MagzineLeftText = MagzineLeftObject.GetComponent<Text>();
        SmgHowManyatonce = GetComponent<WeaponPickingSystem>().SmgHowManyAtOnce;
        RpgHowmanyatOnce = GetComponent<WeaponPickingSystem>().RPGHowManyAtOnce;

        //Debug.Log(MagzineLeftText.GetComponent<Text>().text);
    }
    void RecognizePool()
    {
        PoolControllerObject = FindObjectOfType<SimpleGunObjectPool>().gameObject;
        RoundController = FindObjectOfType<RoundController>();
        CancelInvoke();
    }
    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKey(ShootKey) && Time.time > nextShotTime && CurrentMagzine > 0 && RoundController.Canfire)
        {
            nextShotTime = Time.time + ReloadRate;
            Fire();
        }
    }

    
    void Fire()
    {
        var CurrentGun = GetComponent<WeaponPickingSystem>().WhichGunisOn;
        if (CurrentGun == WeaponPickingSystem.GUN_TYPE.simpleGun)
        {
            PrimarySlotMagzineLeft--;
            CurrentMagzine = PrimarySlotMagzineLeft;
            MagzineLeftText.text = PrimarySlotMagzineLeft.ToString();
            if (isServer) RpcShootSimpleGun();
            else CmdShootSimpleGun();
            Checkformagzine();
            SoundSource.clip = SimpleGun;
            SoundSource.Play();
        }
        else if(CurrentGun == WeaponPickingSystem.GUN_TYPE.secondGun)
        {

            StartCoroutine(SmgThreeShot(GetComponent<WeaponPickingSystem>().SmgShotsDelay,SmgHowManyatonce));
        }
        else if (CurrentGun == WeaponPickingSystem.GUN_TYPE.RifleGun)
        {
            SecondarySlotMagzineLeft--;
            CurrentMagzine = SecondarySlotMagzineLeft;
            MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
            if (isServer) RpcShootRifleGun();
            else CmdShootRifleGun();
            Checkformagzine();
            SoundSource.clip = RifleGun;
            SoundSource.Play();
        }
        else if (CurrentGun == WeaponPickingSystem.GUN_TYPE.SniperGun)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000f))
                {
                    try
                    {
                        if (hit.collider.gameObject.transform.parent.transform.parent.gameObject.GetComponent<SetPlayerName>().PlayerID == GetComponent<SetPlayerName>().PlayerID) return;
                        else
                        {
                            SecondarySlotMagzineLeft--;
                            CurrentMagzine = SecondarySlotMagzineLeft;
                            MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
                            if (isServer) RpcShootSniperGun();
                            else CmdShootSniperGun();
                            Checkformagzine();
                            SoundSource.clip = SniperGun;
                            SoundSource.Play();
                            if (SecondarySlotMagzineLeft == 0) return;
                            Invoke("SniperReloadSound", 0.550f);
                        }
                    }
                    catch
                    {
                        SecondarySlotMagzineLeft--;
                        CurrentMagzine = SecondarySlotMagzineLeft;
                        MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
                        if (isServer) RpcShootSniperGun();
                        else CmdShootSniperGun();
                        Checkformagzine();
                        SoundSource.clip = SniperGun;
                        SoundSource.Play();
                        if (SecondarySlotMagzineLeft == 0) return;
                        Invoke("SniperReloadSound", 0.550f);
                    }
                }
            }
        }
        else if (CurrentGun == WeaponPickingSystem.GUN_TYPE.ShutGun)
        {
            SecondarySlotMagzineLeft--;
            CurrentMagzine = SecondarySlotMagzineLeft;
            MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
            if (isServer) RpcShootShutGun();
            else CmdShootShutGun();
            Checkformagzine();
            SoundSource.clip = ShotGun;
            SoundSource.Play();
            if (SecondarySlotMagzineLeft == 0) return;
            Invoke("ShotGunReloadSound", 0.710f);
        }
        else if (CurrentGun == WeaponPickingSystem.GUN_TYPE.RPG)
        {
            StartCoroutine(RpgThreeShots(GetComponent<WeaponPickingSystem>().RPGShotsDelay, RpgHowmanyatOnce));
        }

        

    }
    
    void Checkformagzine()
    {
        var CurrentGun = GetComponent<WeaponPickingSystem>().WhichGunisOn;

        if (PrimarySlotMagzineLeft == 0 && SecondarySlotMagzineLeft != 0 && CurrentGun == WeaponPickingSystem.GUN_TYPE.simpleGun)
        {
            gameObject.GetComponent<WeaponPickingSystem>().SwitchGun();
        }
        else if (SecondarySlotMagzineLeft == 0 && CurrentGun != WeaponPickingSystem.GUN_TYPE.simpleGun)
        {
            gameObject.GetComponent<WeaponPickingSystem>().NetworkWeaponDiactivate();
            gameObject.GetComponent<WeaponPickingSystem>().SwitchGun();
            gameObject.GetComponent<WeaponPickingSystem>().SecondarySlotOn = false;
            gameObject.GetComponent<WeaponPickingSystem>().SecondaryGImg.enabled = false;

        }
    }


    [Command]
    void CmdShootSimpleGun()
    {
       
        RpcShootSimpleGun();
        /*var bullet = Instantiate(BulletObject, ShotPosition.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        NetworkServer.Spawn(bullet);*/
        
    }
    [ClientRpc]
    void RpcShootSimpleGun()
    {
        //PoolControllerObject = GameObject.Find("PoolController");
        var PlayerId = GetComponent<SetPlayerName>().PlayerID;

        var BulletObject1 = PoolControllerObject.GetComponent<SimpleGunObjectPool>().GetPooledObject();
        if (BulletObject1 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().SGShotPos1;
        BulletObject1.transform.position = ShotPosition1.position;
        BulletObject1.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObject1.GetComponent<ShotProjectile>().PlayerId = PlayerId;
        BulletObject1.SetActive(true);

        var BulletObject2 = PoolControllerObject.GetComponent<SimpleGunObjectPool>().GetPooledObject();
        if (BulletObject2 == null) return;
        ShotPosition2 = gameObject.GetComponent<WeaponPickingSystem>().SGShotPos2;
        BulletObject2.transform.position = ShotPosition2.position;
        BulletObject2.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObject2.GetComponent<ShotProjectile>().PlayerId = PlayerId;
        BulletObject2.SetActive(true);
    }

    IEnumerator SmgThreeShot(float delay, int howmany)
    {
        for(int i=0; i < howmany; i++)
        {
            SecondarySlotMagzineLeft--;
            CurrentMagzine = SecondarySlotMagzineLeft;
            MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
            if (isServer) RpcShootSecondGun();
            else CmdShootSecondGun();
            yield return new WaitForSeconds(delay);
            Checkformagzine();
        }
        SoundSource.clip = SecondGun;
        SoundSource.Play();
    }

    IEnumerator RpgThreeShots(float delay, int howmany)
    {
        for(int i=0; i<howmany; i++)
        {
            SecondarySlotMagzineLeft--;
            CurrentMagzine = SecondarySlotMagzineLeft;
            MagzineLeftText.text = SecondarySlotMagzineLeft.ToString();
            if (isServer) RpcShootRPG();
            else CmdShootRPG();
            Checkformagzine();
            //SoundSource.clip = RPGGunSound;
            //SoundSource.Play();
            yield return new WaitForSeconds(delay);
        }
    }

    [Command]
    void CmdShootSecondGun()
    {
        RpcShootSecondGun();
    }
    [ClientRpc]
    void RpcShootSecondGun()
    {
        //PoolControllerObject = GameObject.Find("PoolController");
        var PlayerId = GetComponent<SetPlayerName>().PlayerID;


        var BulletObject1 = PoolControllerObject.GetComponent<SMGGunBulletPool>().GetSMGGunBullet();
        if (BulletObject1 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().SecondGunShotPoint1;
        BulletObject1.transform.position = ShotPosition1.position;
        BulletObject1.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObject1.GetComponent<ShotProjectile>().PlayerId = PlayerId;
        BulletObject1.SetActive(true);

        var BulletObject2 = PoolControllerObject.GetComponent<SMGGunBulletPool>().GetSMGGunBullet();
        if (BulletObject2 == null) return;
        ShotPosition2 = gameObject.GetComponent<WeaponPickingSystem>().SecondGunShotPoint2;
        BulletObject2.transform.position = ShotPosition2.position;
        BulletObject2.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObject2.GetComponent<ShotProjectile>().PlayerId = PlayerId;
        BulletObject2.SetActive(true);
    }
    
    [Command]
    void CmdShootRifleGun()
    {
        RpcShootRifleGun();
    }
    [ClientRpc]
    void RpcShootRifleGun()
    {
        var playerid = GetComponent<SetPlayerName>().PlayerID;
        var BulletObj = PoolControllerObject.GetComponent<RifleGunBulletPool>().GetRifleBullet();
        if (BulletObj == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().RifleGunShotPoint;
        BulletObj.transform.position = ShotPosition1.position;
        BulletObj.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObj.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj.SetActive(true);
    }

    [Command]
    void CmdShootSniperGun()
    {
        RpcShootSniperGun();
    }
    [ClientRpc]
    void RpcShootSniperGun()
    {
        var PlayerID = GetComponent<SetPlayerName>().PlayerID;
        var BulletObj = PoolControllerObject.GetComponent<SniperGunBulletPool>().GetSniperGunBullet();
        if (BulletObj == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().SniperGunShotPoint;
        
        Debug.Log("Here");
        BulletObj.transform.position = ShotPosition1.position;
        Vector3 BullPos = new Vector3(hit.point.x , ShotPosition1.transform.position.y , hit.point.z);
        BulletObj.transform.LookAt(BullPos);
        BulletObj.GetComponent<Rigidbody>().velocity = BulletObj.transform.forward * BulletSpeed;
        //BulletObj.GetComponent<Rigidbody>().AddForce(BulletObj.transform.forward * BulletSpeed);
        BulletObj.GetComponent<ShotProjectile>().PlayerId = PlayerID;
        BulletObj.SetActive(true);
        //projectile.GetComponent<ETFXProjectileScript>().impactNormal = hit.normal;
        
    }

    [Command]
    void CmdShootShutGun()
    {
        RpcShootShutGun();
    }
    [ClientRpc]
    void RpcShootShutGun()
    {
        var playerid = GetComponent<SetPlayerName>().PlayerID;

        var BulletObj1 = PoolControllerObject.GetComponent<ShutGunBulletPool>().GetShutGunBullet();
        if (BulletObj1 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().ShutGunShotPoint;
        BulletObj1.transform.position = ShotPosition1.position;
        BulletObj1.transform.rotation = ShotPosition1.rotation;
        //BulletObj1.transform.rotation = Quaternion.Euler(0, 40, 0);
        BulletObj1.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, 40, 0) * transform.forward * BulletSpeed;
        BulletObj1.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj1.SetActive(true);

        var BulletObj2 = PoolControllerObject.GetComponent<ShutGunBulletPool>().GetShutGunBullet();
        if (BulletObj2 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().ShutGunShotPoint;
        BulletObj2.transform.position = ShotPosition1.position;
        BulletObj2.transform.rotation = ShotPosition1.rotation;
        //BulletObj2.transform.rotation = Quaternion.Euler(0, 20, 0);
        BulletObj2.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, 20, 0) * transform.forward * BulletSpeed;
        BulletObj2.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj2.SetActive(true);

        var BulletObj3 = PoolControllerObject.GetComponent<ShutGunBulletPool>().GetShutGunBullet();
        if (BulletObj3 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().ShutGunShotPoint;
        BulletObj3.transform.position = ShotPosition1.position;
        BulletObj3.transform.rotation = ShotPosition1.rotation;
        //BulletObj3.transform.rotation = Quaternion.Euler(0, 0, 0);

        BulletObj3.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0,0,0) * transform.forward * BulletSpeed;
        BulletObj3.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj3.SetActive(true);

        var BulletObj4 = PoolControllerObject.GetComponent<ShutGunBulletPool>().GetShutGunBullet();
        if (BulletObj4 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().ShutGunShotPoint;
        BulletObj4.transform.position = ShotPosition1.position;
        BulletObj4.transform.rotation = ShotPosition1.rotation;
        //BulletObj4.transform.rotation = Quaternion.Euler(0, -20, 0);
        BulletObj4.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, -20, 0) * transform.forward * BulletSpeed;
        BulletObj4.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj4.SetActive(true);

        var BulletObj5 = PoolControllerObject.GetComponent<ShutGunBulletPool>().GetShutGunBullet();
        if (BulletObj5 == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().ShutGunShotPoint;
        BulletObj5.transform.position = ShotPosition1.position;
        BulletObj5.transform.rotation = ShotPosition1.rotation;
        //BulletObj5.transform.rotation = Quaternion.Euler(0, -40, 0);
        BulletObj5.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, -40, 0) * transform.forward * BulletSpeed;
        BulletObj5.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj5.SetActive(true);

        
    }

    [Command]
    void CmdShootRPG()
    {
        RpcShootRPG();
    }
    [ClientRpc]
    void RpcShootRPG()
    {
        var playerid = GetComponent<SetPlayerName>().PlayerID;
        var BulletObj = PoolControllerObject.GetComponent<RPGBulletPool>().GetRPGBullet();
        if (BulletObj == null) return;
        ShotPosition1 = gameObject.GetComponent<WeaponPickingSystem>().RPGShotPoint;
        BulletObj.transform.position = ShotPosition1.position;
        BulletObj.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
        BulletObj.GetComponent<ShotProjectile>().PlayerId = playerid;
        BulletObj.SetActive(true);
    }
    public void SniperReloadSound()
    {
        SoundSource.clip = SniperGunReload;
        SoundSource.Play();
    }
    public void ShotGunReloadSound()
    {
        SoundSource.clip = ShotGunReload;
        SoundSource.Play();
    }
}
