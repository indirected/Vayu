using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauseDamage : MonoBehaviour {
    public int MaxDamage = 10;
    public int MinDamage = 0;
    public int Damage;
	// Use this for initialization
	void Start () {
        Damage = Random.Range(MinDamage, MaxDamage);
	}
	public int GetDamage()
    {
        return MaxDamage;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
