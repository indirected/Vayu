using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;




public class OffRoad : MonoBehaviour {
    public float LimitedSpeedValue;
    private float m_topspeed;

    

    
    private void OnTriggerEnter(Collider other)
    {

        try
        {
            var GameObj = other.transform.parent.transform.parent;
            if (GameObj == null) return;
            if (GameObj.tag == "Player")
            {
                StartCoroutine(LimitSpeed(GameObj.gameObject));
            }
        }
        catch { }
    }
    private void OnTriggerExit(Collider other)
    {
        try
        {
            var GameObj = other.transform.parent.transform.parent;
            if (GameObj == null) return;
            if (GameObj.tag == "Player")
            {
                StartCoroutine(ExitLimit(GameObj.gameObject));
            }
        }
        catch { }
    }
    IEnumerator LimitSpeed(GameObject go)
    {
        var TopSpeed = go.GetComponent<CarController>().m_Topspeed;
        m_topspeed = TopSpeed;
        var MustLimit = TopSpeed - LimitedSpeedValue;
        if (MustLimit < 0) yield return null;
        TopSpeed -= (MustLimit / 3);
        go.GetComponent<CarController>().m_Topspeed = TopSpeed;
        yield return new WaitForSecondsRealtime(.1f);
        TopSpeed -= (MustLimit / 3);
        yield return new WaitForSecondsRealtime(.1f);
        TopSpeed -= (MustLimit / 3);
        go.GetComponent<CarController>().m_Topspeed = TopSpeed;
        print("Limited");
        yield return null;
    }
    IEnumerator ExitLimit(GameObject go)
    {
        var CurrentSpeed = go.GetComponent<CarController>().m_Topspeed;
        var MustLimit = m_topspeed - LimitedSpeedValue;
        if (MustLimit < 0) yield return null;
        CurrentSpeed += (MustLimit);
        go.GetComponent<CarController>().m_Topspeed = CurrentSpeed;
        yield return null;
    }
}
