using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplotionCreation : MonoBehaviour {

    private SimpleandSMGExplotionPool ExplotionPoolController;
    private GameObject explotion;

    // Use this for initialization
    void Start () {
        ExplotionPoolController = FindObjectOfType<SimpleandSMGExplotionPool>();

    }
    ShotProjectile ProjectileScript;
    GameObject other;
    int id;
    private void OnCollisionEnter(Collision collision)
    {
        other = collision.gameObject;
        if (other.tag == "Bullet")
        {
            ProjectileScript = other.GetComponent<ShotProjectile>();
            id = ProjectileScript.PlayerId;
            if (id == GetComponent<SetPlayerName>().PlayerID) return;
            if (ProjectileScript.BulletType == ShotProjectile.Bullet_TYPE.SimpleandSMG)
            {
                explotion = ExplotionPoolController.GetSimpleExplotion();
                explotion.transform.position = other.transform.position;
                explotion.SetActive(true);
            }

            //KillerId = ProjectileScript.PlayerId;
            //HealthScript.TakeDamage(ProjectileScript.DamageValue, KillerId);
            // print("Hit: " + Damage);
            //print("Currenthealth= " + gameObject.GetComponent<Health>().CurrentHealth);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}
