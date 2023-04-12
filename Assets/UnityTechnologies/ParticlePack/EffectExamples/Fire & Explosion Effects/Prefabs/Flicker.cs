using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public Light _light;
    public float minTime;
    public float maxTime;
    public float Timer;

    public float minIntensity;
    public float maxIntensity;
    public AudioSource audioSource;

    private void Start()
    {
        Timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {
        if (Timer > 0)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            _light.intensity = Random.Range(minIntensity, maxIntensity);
            Timer = Random.Range(minTime, maxTime);
        }
    }
}
