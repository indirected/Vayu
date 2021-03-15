using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplotionDis : MonoBehaviour {

    public float ExplotionLiveTime = 0.5f;

    private void OnEnable()
    {
        Invoke("DisExplotion", ExplotionLiveTime);
    }
    void DisExplotion()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
