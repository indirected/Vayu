using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    Camera m_Camera;
	// Use this for initialization
	void Start () {
        Invoke("CameraSet", 0.7f);
        
	}
	
	// Update is called once per frame
    
	void Update () {
        if (m_Camera == null) return;
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                         m_Camera.transform.rotation * Vector3.up);
        //Look At Camera For Canvases
    }
    void CameraSet()
    {
        m_Camera = Camera.main;
    }
}
