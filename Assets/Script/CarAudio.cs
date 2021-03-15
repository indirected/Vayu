using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour {
    public AudioClip lowAccelClip;// Audio clip for low acceleration
    public AudioClip highAccelClip; // Audio clip for high acceleration
    public AudioSource CarSource; // Source for the Engine sounds

    public int maxRolloffDistance;
    Rigidbody rig;
    bool IsPlaying;
    int gear;
    // Use this for initialization
    void Start () {
        rig = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        var localvel = transform.InverseTransformDirection(rig.velocity);
        if (localvel.z < 50 && localvel.z > 0)
        {
            if (IsPlaying && gear == 1) return;
            CarSource.Stop();
            CarSource.clip = lowAccelClip;
            CarSource.Play();
            gear = 1;
            IsPlaying = true;
        }
        else if (localvel.z > 50 && localvel.z < 100)
        {
            if (IsPlaying && gear == 2) return;
            CarSource.Stop();
            CarSource.clip = highAccelClip;
            CarSource.Play();
            gear = 2;
            IsPlaying = true;
        }
        else if (localvel.z < 0)
        {
            if (IsPlaying && gear == -1) return;
            CarSource.Stop();
            CarSource.clip = lowAccelClip;
            CarSource.Play();
            gear = -1;
            IsPlaying = true;
        }
        else {
            CarSource.Stop();
            IsPlaying = false;
        } 
    }
    private void StopSound()
    {

        IsPlaying = false;
    }
    IEnumerator Wait (float lenghth)
    {
        yield return new WaitForSeconds(lenghth);
        yield return null;
    }
}
