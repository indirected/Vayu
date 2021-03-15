using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEffect : MonoBehaviour {
    public GameObject TrailPrefab;
    public Transform TrailPosition;
    // For Not Destroy The Trail Position Transform
    public Transform TrailDatached;
    private bool Skidding =false;
	// Use this for initialization
	void Start () {
		if(TrailDatached == null)
        {
            TrailDatached = new GameObject("Trail - Datached").transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponentInParent<MovingOptions>().Drifting && !Skidding )
        {
            StartCoroutine(StartTrails());           
        }
	}
    IEnumerator StartTrails()
    {
        
        TrailPosition = Instantiate(TrailPrefab).transform;
        while(TrailPrefab == null || Skidding == true){
            yield return null;
        }
        TrailPosition.parent = transform;
        TrailPosition.localPosition =  Vector3.up * GetComponent<WheelCollider>().radius;
        Skidding = true;
        // Destroy Trail After 3 Seconds
        yield return new WaitForSeconds(2);
        StartCoroutine(StopTrails());
    }
    IEnumerator StopTrails()
    {
        // Check If Skidding !
        if (Skidding == false) yield return null;
        //Change The parent Of Trail
        TrailPosition.parent = TrailDatached.transform;
        //Destroy GameObject Of Trail
       //Destroy(TrailPrefab.gameObject , 10);
        Skidding = false;
    }

}
