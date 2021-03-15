using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LookAtMouse : NetworkBehaviour {
    Quaternion TargetRotation;
    public float TurnSpeed = 5;
    private void Update()
    {
        if (!isLocalPlayer) return;
        SetRotation();
        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, TurnSpeed * Time.deltaTime);
    }
    void SetRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if(plane.Raycast(ray , out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            TargetRotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
