using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubeControll : NetworkBehaviour {

    //variables for moving
    public float MoveSpeed = 3f;
    public float RotateSpeed = 150f;

    //variables for shooting
    public GameObject ShotPrefab;
    public Transform Shotpawn;
    public float ShotSpeed;
    public float RealoadSpeed = 0.5f;
    private float _nextShotTime;
	// Use this for initialization
	void Start () {
		
	}
    public override void OnStartLocalPlayer()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * RotateSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextShotTime)
        {
            CmdFire();
            _nextShotTime = Time.time + RealoadSpeed;
        }



    }

    [Command]
    void CmdFire()
    {
        
        var bullet = Instantiate(ShotPrefab, Shotpawn.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * ShotSpeed;
        NetworkServer.Spawn(bullet);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.tag == "Bullet")
        {
            var Damage = other.GetComponent<ShotProjectile>().DamageValue;
            //gameObject.GetComponent<Health>().TakeDamage(Damage);
            print("Hit: " + Damage);
            print("Currenthealth= " + gameObject.GetComponent<Health>().CurrentHealth);
        }
    }
}
