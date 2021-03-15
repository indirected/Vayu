using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeaponPickingSystem : NetworkBehaviour {

    //to recognize activated weapon
    public enum GUN_TYPE
    {
        simpleGun,
        secondGun,
        RifleGun,
        SniperGun,
        ShutGun,
        RPG
    };
    public GUN_TYPE WhichGunisOn, WhichGunisPicked;
    [HideInInspector]
    public bool SecondarySlotOn = false;



    //public Transform SimpleGunPosition;
    //private GameObject StarterGun;

    
    //private GameObject[] AllPlayers;
    


    //Required Level Variables
    public int SimpleGunLevel;
    public int SmgGunLevel;
    public int RifleGunLevel;
    public int SniperGunLevel;
    public int ShutGunLevel;
    public int RPGLevel;

    [HideInInspector]
    public bool SecondaryGunDiactive = false;

    //Simple Gun Variables
    public GameObject SimpleGun;
    //public GameObject SGBullet;
    public Transform SGShotPos1;
    public Transform SGShotPos2;
    public float SGBulletSpeed;
    public float SGReloadRate;
    public int SGMagzine;
    

    //Secondary Gun Variables
    public GameObject SecondGun;
    //public GameObject SecondGunBullet;
    public Transform SecondGunShotPoint1;
    public Transform SecondGunShotPoint2;
    public Sprite SMGImage;
    public float SecondGBulletSpeed;
    public float SmgShotsDelay;
    public int SmgHowManyAtOnce = 3;
    public float SecondGReloadRate;
    public int SecondGMagzine;

    //Rifle Gun Variables
    public GameObject RifleGun;
    public Transform RifleGunShotPoint;
    public Sprite RifleImage;
    public float RifleGunBulletSpeed;
    public float RifleGunReloadRate;
    public int RifleGunMagzine;


    //SniperGun Variables
    public GameObject SniperGun;
    public Transform SniperGunShotPoint;
    public Sprite SniperImage;
    public float SniperGunBulletSpeed;
    public float SniperGunReloadRate;
    public int SniperGunMagzine;
    public int CameraAdedHight;
    public float CameraMoveSmoothnes;
    private bool IsCameraUp = false;

    //ShutGun Variables
    public GameObject ShutGun;
    public Transform ShutGunShotPoint;
    public Sprite ShutGunImage;
    public float ShutGunBulletSpeed;
    public float ShutGunReloadRate;
    public int ShutGunMagzine;

    //RPG Variables
    public GameObject RPG;
    public Transform RPGShotPoint;
    public Sprite RPGImage;
    public float RPGBulletSpeed;
    public float RPGShotsDelay;
    public int RPGHowManyAtOnce = 3;
    public float RPGReloadRate;
    public int RPGMagzine;


    //UI and Key Variables
    public KeyCode SwitchKey1;
    public KeyCode SwitchKey2;
    public float DisAlpha = 0.4f;
    public float EnAlpha = 1f;
    private Image PrimaryGImg;
    [HideInInspector]
    public Image SecondaryGImg;
    

    
    // Use this for initialization
    void Start()
    {
        PrimaryGImg = GameObject.Find("PrimaryGunImg").GetComponent<Image>();
        SecondaryGImg = GameObject.Find("SecondaryGunImg").GetComponent<Image>();
   


        ActivateSimpleGun();
        //if (!isLocalPlayer) return;
        //CmdStarterGunCreate();
        //AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        /*for (int i=0; i < AllPlayers.Length; i++)
        {
            var CheckingPlayer = AllPlayers[i].GetComponent<WeaponPickingSystem>();
            if (!CheckingPlayer.isLocalPlayer)
            {
                if (CheckingPlayer.WhichGunisOn == GUN_TYPE.simpleGun)
                {
                    CheckingPlayer.SimpleGun.SetActive(true);
                }
                if (CheckingPlayer.WhichGunisOn == GUN_TYPE.secondGun)
                {
                    CheckingPlayer.SecondGun.SetActive(true);
                }
            }
            
        }*/

    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(SwitchKey1) || Input.GetKeyDown(SwitchKey2))
        {
            SwitchGun();
        }
    }



    //SimpleGun Activate
    public void ActivateSimpleGun()
    {
        
        //StartCoroutine(ActiveSGRoutine());
        
        gameObject.GetComponent<ShootControll>().PrimarySlotMagzineLeft = SGMagzine;
        gameObject.GetComponent<ShootControll>().CurrentMagzine = SGMagzine;
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = SGMagzine.ToString();
        //PrimarySlotGun = GUN_TYPE.simpleGun;
        //PrimarySlotFull = true;
        //SecondGunDiactive = false;
        SetSGPropertis();
    } 
    public void SetSGPropertis()
    {
        gameObject.GetComponent<ShootControll>().BulletSpeed = SGBulletSpeed;
        gameObject.GetComponent<ShootControll>().ReloadRate = SGReloadRate;
        //gameObject.GetComponent<ShootControll>().BulletObject = SGBullet;
        if (isServer)
        {
            gameObject.GetComponent<ShootControll>().ShotPosition1 = SGShotPos1;
            gameObject.GetComponent<ShootControll>().ShotPosition2 = SGShotPos2;
        }
        else CmdSetSGShotPoint();
        //SGImgAlpha.a = EnAlpha;
        //PrimaryGImg.color = SGImgAlpha;
        PrimaryGImg.color = new Color(PrimaryGImg.color.r, PrimaryGImg.color.g, PrimaryGImg.color.b, EnAlpha);
        WhichGunisOn = GUN_TYPE.simpleGun;
        
    }

    [Command]
    void CmdSetSGShotPoint()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = SGShotPos1;
        gameObject.GetComponent<ShootControll>().ShotPosition2 = SGShotPos2;
    }

    IEnumerator ActiveSGRoutine()
    {
        //NetworkWeaponDiactivate();
        //yield return new WaitUntil(() => AllGunsDiactive == true);
        if (isServer) RpcActiveSG();
        else CmdActiveSG();
        yield return null;

    }

    [Command]
    void CmdActiveSG()
    {
       
        SimpleGun.SetActive(true);
        RpcActiveSG();
        
    }
    [ClientRpc]
    void RpcActiveSG()
    {
        SimpleGun.SetActive(true);
        //SecondaryGunDiactive = false;
        
    }

    //SeconGun Activation
    public void ActivateSecondGun()
    {
        WhichGunisPicked = GUN_TYPE.secondGun;
        StartCoroutine(ActiveSecondeGunRoutine());
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = SecondGMagzine.ToString();
        gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft = SecondGMagzine;
        SecondaryGunDiactive = false;
        SecondarySlotOn = true;
        if (WhichGunisOn == GUN_TYPE.simpleGun)
        {
            SwitchGun();
        }
        SetSecondaryGunPropertis();

    }

    //RifleGun Activation
    public void ActivateRifleGun()
    {
        WhichGunisPicked = GUN_TYPE.RifleGun;
        StartCoroutine(ActiveSecondeGunRoutine());
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = RifleGunMagzine.ToString();
        gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft = RifleGunMagzine;
        SecondaryGunDiactive = false;
        SecondarySlotOn = true;
        if (WhichGunisOn == GUN_TYPE.simpleGun) SwitchGun();
        SetSecondaryGunPropertis();

    }

    //SniperGun Activation
    public void ActivateSniperGun()
    {
        WhichGunisPicked = GUN_TYPE.SniperGun;
        StartCoroutine(ActiveSecondeGunRoutine());
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = SniperGunMagzine.ToString();
        gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft = SniperGunMagzine;
        SecondaryGunDiactive = false;
        SecondarySlotOn = true;
        if (WhichGunisOn == GUN_TYPE.simpleGun) SwitchGun();
        SetSecondaryGunPropertis();
        StartCoroutine(SetCameraY());
    }

    IEnumerator SetCameraY()
    {
        var YFromCamera = gameObject.GetComponent<CameraSet>().DistanceFromGameObject.y;
        if (!IsCameraUp)
        {
            for(int i=0; i < CameraMoveSmoothnes; i++)
            {
                gameObject.GetComponent<CameraSet>().DistanceFromGameObject.y += CameraAdedHight / CameraMoveSmoothnes;
                IsCameraUp = true;
                yield return new WaitForSecondsRealtime(0.01f);

            }
        }
        else
        {
            for (int i = 0; i < CameraMoveSmoothnes; i++)
            {
                gameObject.GetComponent<CameraSet>().DistanceFromGameObject.y -= CameraAdedHight / CameraMoveSmoothnes;
                IsCameraUp = false;
                yield return new WaitForSecondsRealtime(0.01f);

            }
        }
    }

    //ShutGun Activation
    public void ActivateShutGun()
    {
        WhichGunisPicked = GUN_TYPE.ShutGun;
        StartCoroutine(ActiveSecondeGunRoutine());
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = ShutGunMagzine.ToString();
        gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft = ShutGunMagzine;
        SecondaryGunDiactive = false;
        SecondarySlotOn = true;
        if (WhichGunisOn == GUN_TYPE.simpleGun) SwitchGun();
        SetSecondaryGunPropertis();
    }

    public void ActivateRPG()
    {
        WhichGunisPicked = GUN_TYPE.RPG;
        StartCoroutine(ActiveSecondeGunRoutine());
        gameObject.GetComponent<ShootControll>().MagzineLeftText.text = RPGMagzine.ToString();
        gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft = RPGMagzine;
        SecondaryGunDiactive = false;
        SecondarySlotOn = true;
        if (WhichGunisOn == GUN_TYPE.simpleGun) SwitchGun();
        SetSecondaryGunPropertis();
    }
    




    public void SetSecondaryGunPropertis()
    {
        
        if(WhichGunisPicked == GUN_TYPE.secondGun && isLocalPlayer)
        {

            gameObject.GetComponent<ShootControll>().BulletSpeed = SecondGBulletSpeed;
            gameObject.GetComponent<ShootControll>().ReloadRate = SecondGReloadRate;
            //gameObject.GetComponent<ShootControll>().BulletObject = SecondGunBullet;
            if (isServer)
            {
                gameObject.GetComponent<ShootControll>().ShotPosition1 = SecondGunShotPoint1;
                gameObject.GetComponent<ShootControll>().ShotPosition2 = SecondGunShotPoint2;
            }
            else CmdChangeShotposfor2ndGun();
            SecondaryGImg.sprite = SMGImage;
            //SGImgAlpha.a = EnAlpha;
            //PrimaryGImg.color = SGImgAlpha;
            SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, EnAlpha);
            SecondaryGImg.enabled = true;
            WhichGunisOn = GUN_TYPE.secondGun;
        }
        else if (WhichGunisPicked == GUN_TYPE.RifleGun && isLocalPlayer)
        {
            gameObject.GetComponent<ShootControll>().BulletSpeed = RifleGunBulletSpeed;
            gameObject.GetComponent<ShootControll>().ReloadRate = RifleGunReloadRate;
            //gameObject.GetComponent<ShootControll>().BulletObject = SecondGunBullet;
            if (isServer)
            {
                gameObject.GetComponent<ShootControll>().ShotPosition1 = RifleGunShotPoint;
            }
            else CmdChangeShotposforRifleGun();
            SecondaryGImg.sprite = RifleImage;
            //SGImgAlpha.a = EnAlpha;
            //PrimaryGImg.color = SGImgAlpha;
            SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, EnAlpha);
            SecondaryGImg.enabled = true;
            WhichGunisOn = GUN_TYPE.RifleGun;
        }
        else if (WhichGunisPicked == GUN_TYPE.SniperGun && isLocalPlayer)
        {
            gameObject.GetComponent<ShootControll>().BulletSpeed = SniperGunBulletSpeed;
            gameObject.GetComponent<ShootControll>().ReloadRate = SniperGunReloadRate;
            if (isServer)
            {
                gameObject.GetComponent<ShootControll>().ShotPosition1 = SniperGunShotPoint;
            }
            else CmdChangeShotposforSniperGun();
            SecondaryGImg.sprite = SniperImage;
            //SGImgAlpha.a = EnAlpha;
            //PrimaryGImg.color = SGImgAlpha;
            SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, EnAlpha);
            SecondaryGImg.enabled = true;
            WhichGunisOn = GUN_TYPE.SniperGun;
        }
        else if (WhichGunisPicked == GUN_TYPE.ShutGun && isLocalPlayer)
        {
            gameObject.GetComponent<ShootControll>().BulletSpeed = ShutGunBulletSpeed;
            gameObject.GetComponent<ShootControll>().ReloadRate = ShutGunReloadRate;
            if (isServer)
            {
                gameObject.GetComponent<ShootControll>().ShotPosition1 = ShutGunShotPoint;
            }
            else CmdChangeShotposforShutGun();
            SecondaryGImg.sprite = ShutGunImage;
            //SGImgAlpha.a = EnAlpha;
            //PrimaryGImg.color = SGImgAlpha;
            SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, EnAlpha);
            SecondaryGImg.enabled = true;
            WhichGunisOn = GUN_TYPE.ShutGun;
        }
        else if (WhichGunisPicked == GUN_TYPE.RPG && isLocalPlayer)
        {
            gameObject.GetComponent<ShootControll>().BulletSpeed = RPGBulletSpeed;
            gameObject.GetComponent<ShootControll>().ReloadRate = RPGReloadRate;
            if (isServer)
            {
                gameObject.GetComponent<ShootControll>().ShotPosition1 = RPGShotPoint;
            }
            else CmdChangeShotposforRPG();
            SecondaryGImg.sprite = RPGImage;
            //SGImgAlpha.a = EnAlpha;
            //PrimaryGImg.color = SGImgAlpha;
            SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, EnAlpha);
            SecondaryGImg.enabled = true;
            WhichGunisOn = GUN_TYPE.RPG;
        }
    }

    [Command]
    void CmdChangeShotposfor2ndGun()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = SecondGunShotPoint1;
        gameObject.GetComponent<ShootControll>().ShotPosition2 = SecondGunShotPoint2;

    }

    [Command]
    void CmdChangeShotposforRifleGun()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = RifleGunShotPoint;
    }

    [Command]
    void CmdChangeShotposforSniperGun()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = SniperGunShotPoint;
    }

    [Command]
    void CmdChangeShotposforShutGun()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = ShutGunShotPoint;
    }

    [Command]
    void CmdChangeShotposforRPG()
    {
        gameObject.GetComponent<ShootControll>().ShotPosition1 = RPGShotPoint;

    }



    IEnumerator ActiveSecondeGunRoutine()
    {
        NetworkWeaponDiactivate();
        yield return new WaitUntil(() => SecondaryGunDiactive == true);
        if(WhichGunisPicked == GUN_TYPE.secondGun)
        {
            if (isServer) RpcActivateSMGGun();
            else CmdActivateSMGGun();
        }
        else if (WhichGunisPicked == GUN_TYPE.RifleGun)
        {
            if (isServer) RpcActivateRifleGun();
            else CmdActivateRifleGun();
        }
        else if (WhichGunisPicked == GUN_TYPE.SniperGun)
        {
            if (isServer) RpcActivateSniperGun();
            else CmdActivateSniperGun();
        }
        else if (WhichGunisPicked == GUN_TYPE.ShutGun)
        {
            if (isServer) RpcActivateShutGun();
            else CmdActivateShutGun();
        }
        else if (WhichGunisPicked == GUN_TYPE.RPG)
        {
            if (isServer) RpcActivateRPG();
            else CmdActivateRPG();
        }
        //yield return null;



    }
    [Command]
    void CmdActivateSMGGun()
    {
        SecondGun.SetActive(true);
        
        RpcActivateSMGGun();
    }
    [ClientRpc]
    void RpcActivateSMGGun()
    {
        SecondGun.SetActive(true);
        SecondaryGunDiactive = false;
        
    }


    [Command]
    void CmdActivateRifleGun()
    {
        RifleGun.SetActive(true);
        RpcActivateRifleGun();
    }
    [ClientRpc]
    void RpcActivateRifleGun()
    {
        RifleGun.SetActive(true);
        SecondaryGunDiactive = false;
    }

    [Command]
    void CmdActivateSniperGun()
    {
        SniperGun.SetActive(true);
        RpcActivateSniperGun();
    }
    [ClientRpc]
    void RpcActivateSniperGun()
    {
        SniperGun.SetActive(true);
        SecondaryGunDiactive = false;
    }

    [Command]
    void CmdActivateShutGun()
    {
        ShutGun.SetActive(true);
        RpcActivateShutGun();
    }
    [ClientRpc]
    void RpcActivateShutGun()
    {
        ShutGun.SetActive(true);
        SecondaryGunDiactive = false;
    }

    [Command]
    void CmdActivateRPG()
    {
        RPG.SetActive(true);
        RpcActivateRPG();
    }
    [ClientRpc]
    void RpcActivateRPG()
    {
        RPG.SetActive(true);
        SecondaryGunDiactive = false;
    }



    //Gun Switching
    public void SwitchGun()
    {
        if (SecondarySlotOn == true)
        {
            if (WhichGunisOn == GUN_TYPE.simpleGun)
            {
                PrimaryGImg.color = new Color(PrimaryGImg.color.r, PrimaryGImg.color.g, PrimaryGImg.color.b, DisAlpha);
                SetSecondaryGunPropertis();
                gameObject.GetComponent<ShootControll>().MagzineLeftText.text = gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft.ToString();
                gameObject.GetComponent<ShootControll>().CurrentMagzine = gameObject.GetComponent<ShootControll>().SecondarySlotMagzineLeft;
            }
            else 
            {
                SecondaryGImg.color = new Color(SecondaryGImg.color.r, SecondaryGImg.color.g, SecondaryGImg.color.b, DisAlpha);
                SetSGPropertis();
                gameObject.GetComponent<ShootControll>().MagzineLeftText.text = gameObject.GetComponent<ShootControll>().PrimarySlotMagzineLeft.ToString();
                gameObject.GetComponent<ShootControll>().CurrentMagzine = gameObject.GetComponent<ShootControll>().PrimarySlotMagzineLeft;

            }
        }
        
    }


    public void NetworkWeaponDiactivate()
    {
        if (SniperGun.activeInHierarchy) StartCoroutine(SetCameraY());

        if (isServer)
        {
            RpcDeactivateWeapons();
        }
        else
        {
            CmdDeactivateWeapons();
        }
    }
    

    [ClientRpc]
    public void RpcDeactivateWeapons()
    {
        if (SecondGun.activeInHierarchy) SecondGun.SetActive(false);
        else if (RifleGun.activeInHierarchy) RifleGun.SetActive(false);
        else if (SniperGun.activeInHierarchy) SniperGun.SetActive(false);
        else if (ShutGun.activeInHierarchy) ShutGun.SetActive(false);
        else if (RPG.activeInHierarchy) RPG.SetActive(false);
        SecondaryGunDiactive = true;
    }

    [Command]
    public void CmdDeactivateWeapons()
    {
        if (SecondGun.activeInHierarchy) SecondGun.SetActive(false);
        else if (RifleGun.activeInHierarchy) RifleGun.SetActive(false);
        else if (SniperGun.activeInHierarchy) SniperGun.SetActive(false);
        else if (ShutGun.activeInHierarchy) ShutGun.SetActive(false);
        else if (RPG.activeInHierarchy) RPG.SetActive(false);
        RpcDeactivateWeapons();
    }


    /*public void DisableWepoan()
    {

        if (WhichGunisPicked == GUN_TYPE.secondGun) SecondGun.SetActive(false);
        else if (WhichGunisPicked == GUN_TYPE.RifleGun) RifleGun.SetActive(false);
        SecondaryGunDiactive = true;
    }*/


    





    /*public override void OnStartAuthority()
    {
        CmdStarterGunCreate();
        //StarterGun.transform.SetParent(gameObject.transform);
        //StarterGun.transform.Rotate(90, 0, 0);
    }

    
    [Command]
    void CmdStarterGunCreate()
    {

        StarterGun = Instantiate(SimpleGun, SimpleGunPosition) as GameObject;
        NetworkServer.Spawn(StarterGun);

    }*/







}
