using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotProjectile : MonoBehaviour {

    public enum Bullet_TYPE
    {
        SimpleandSMG,

    };

    public Bullet_TYPE BulletType;

    public int DamageValue = 10;
    public float BulletLiveTime;
    public int PlayerId;
    

    private void OnEnable()
    {
        Invoke("Destroy", BulletLiveTime);
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag != "Bullet" )
        {
            try
            {
                if (other.gameObject.GetComponent<SetPlayerName>().PlayerID != PlayerId) return;
                else Destroy();
            }
            catch
            {
                Destroy();
            }
        }
        //print(other.gameObject.name);
    }
    
    void Destroy()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
    

    private void OnDisable()
    {
        CancelInvoke();
    }
}
