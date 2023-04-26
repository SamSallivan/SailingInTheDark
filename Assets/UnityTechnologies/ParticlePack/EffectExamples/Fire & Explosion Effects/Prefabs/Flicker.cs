using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class Flicker : MonoBehaviour
{
    public VolumetricLightBeamSD _light;
    public float minTime;
    public float maxTime;
    public float Timer;

    private void Start()
    {
        Timer = Random.Range(minTime, maxTime);
        StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            _light.enabled = true;
            yield return new WaitForSeconds(Timer);
            Timer = Random.Range(minTime, maxTime);
            _light.enabled = false;
            yield return new WaitForSeconds(Timer / 3f);
        }
    }
}
