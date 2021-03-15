using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarRotation : MonoBehaviour {
    public float Speed;
    float Direction =1;
    Vector3 LastPos;
    Vector3 CurrentPos = new Vector3(0,0,0);
    Vector3 DeltaPos;
    float RotateVal = 1;
    public float Damping;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Button Button1 = FindObjectOfType<Classes>().DPSButton;
            
            CurrentPos.x = Input.mousePosition.x;
            DeltaPos.x = LastPos.x - CurrentPos.x;
            LastPos.x = CurrentPos.x;
            if (DeltaPos.x > 5)
            {
                RotateVal = 2;
                Direction = 1;

            }
            else if (DeltaPos.x < -5)
            {
                RotateVal = -2;
                Direction = -1;
            }
            transform.Rotate(0, DeltaPos.x * Time.deltaTime * Damping , 0);
        }
        else
        {
            RotateVal = Direction;
            transform.Rotate(0, Direction * Speed * Time.deltaTime, 0);
            LastPos.x = Input.mousePosition.x;
        }
    }
    private void OnMouseDrag()
    {
        /*
        CurrentPos.x = Input.mousePosition.x;
        DeltaPos.x = LastPos.x - CurrentPos.x;
        LastPos.x = CurrentPos.x ;
        if (DeltaPos.x > 0) RotateVal = 180;
        else if (DeltaPos.x < 0) RotateVal = -180;
        SecondRotQ = Quaternion.Euler( 0 , transform.rotation.y+ RotateVal  , 0 );
        transform.rotation  = Quaternion.RotateTowards(transform.rotation, SecondRotQ, Time.deltaTime * Damping);
        //transform.eulerAngles = new  Vector3(0, RotateVal, 0);*/
    }
}
