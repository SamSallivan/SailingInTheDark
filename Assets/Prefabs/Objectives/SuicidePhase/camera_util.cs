using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class camera_util : MonoBehaviour
{
    public CinemachineImpulseSource shakeSource;

    public void Shake()
    {
        shakeSource.GenerateImpulse();
    }
}
