using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeTest : MonoBehaviour
{

    CinemachineImpulseSource source;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }
    }
}
