using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    public GameObject Bullet;
    public Transform SpawnBulletPosition;
    public float BulletSpeed;
    public float NextReloadTime;
    private float _nextshot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && _nextshot <= Time.time)
        {
            Fire();
        }
	}

    // [Command]
    public void Fire()
    {
       _nextshot = Time.time + NextReloadTime;
       var Bull = Instantiate(Bullet, SpawnBulletPosition.position, Quaternion.identity);
       Bull.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
    }
}
