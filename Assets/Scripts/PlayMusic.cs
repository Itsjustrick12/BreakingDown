using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusic : MonoBehaviour
{
    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }
}
