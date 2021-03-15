using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour {

    public bool IsRespawnable = true;
    public IEnumerator ResetBool()
    {
        yield return new WaitForSecondsRealtime(5);
        IsRespawnable = true;
    }
}
