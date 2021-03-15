using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public int DestroyAfter;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, DestroyAfter);
	}
    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
