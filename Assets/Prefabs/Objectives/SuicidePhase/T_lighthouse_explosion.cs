using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class C_lighthouse_explosion : T_Cutscene
{
    public Light _light;
    private int minLight = 5000;
    private int maxLight = 200000;
    private float t = 0;

    public AudioSource explosionSFX;
    public CinemachineImpulseSource shakeSource;
    public bool startExplosion = false;

    // private void Start()
    // {
    //     shakeSource = GetComponent<CinemachineImpulseSource>();
    // }

    public override IEnumerator StartCutscene()
    {
        PlayerController.instance.enableMovement = false;

        _light.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        while (t < 1f)
        {
            t += 0.8f * Time.deltaTime;
            if (t >= 0.1f && !startExplosion)
            {
                startExplosion = true;
                shakeSource.GenerateImpulse();
            }
            if (t >= 0.3f)
            {
                explosionSFX.gameObject.SetActive(true);
            }

            _light.intensity = EaseInOutBounce(0, maxLight, t);
            yield return null;
        }
        t = 0;
        while (t < 1f)
        {
            t += 2f * Time.deltaTime;
            _light.intensity = EaseInBounce(maxLight, 0, t);
            yield return null;
        }
        _light.gameObject.SetActive(false);
        explosionSFX.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        EndCutscene();
    }

    //easing functions
    public static float EaseInOutBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
        else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
    }

    public static float EaseInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - EaseOutBounce(0, end, d - value) + start;
    }

    public static float EaseOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1 / 2.75f))
        {
            return end * (7.5625f * value * value) + start;
        }
        else if (value < (2 / 2.75f))
        {
            value -= (1.5f / 2.75f);
            return end * (7.5625f * (value) * value + .75f) + start;
        }
        else if (value < (2.5 / 2.75))
        {
            value -= (2.25f / 2.75f);
            return end * (7.5625f * (value) * value + .9375f) + start;
        }
        else
        {
            value -= (2.625f / 2.75f);
            return end * (7.5625f * (value) * value + .984375f) + start;
        }
    }

}
