using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class BoatAudio : MonoBehaviour
{
    [Foldout("AudioSources", true)]
    public AudioSource light;
    public AudioSource tape;
    public AudioSource anchor;
    public AudioSource ambianceWood;
    public AudioSource ambianceSailing;
    public AudioSource waves;
    public AudioSource other;

    [Foldout("AudioClips", true)]
    public AudioClip sfx_anchor_up;
    public AudioClip sfx_anchor_down;
    public AudioClip sfx_boat_drive_fast; //X
    public AudioClip sfx_boat_drive_slow; //X
    public AudioClip sfx_boat_turn;
    public AudioClip sfx_engine;
    public AudioClip sfx_gear_switch;
    public AudioClip sfx_light;
    public AudioClip sfx_tape_start;
    public AudioClip sfx_tape_playing;
    public AudioClip sfx_tape_end;
    public AudioClip sfx_wood_creak;
    public AudioClip sfx_waves_1;
    public AudioClip sfx_waves_2; //not in use

    [Foldout("References", true)]
    //public I_Helm helm;

    private float randomAmbianceTimer = 0f;

    private void Start()
    {
        ambianceSailing.Play();
        randomAmbianceTimer = Random.Range(10f, 25f);
        PlayWavesAmbiance();
    }

    private void Update()
    {
        UpdateSailingAmbiance(BoatController.instance.helm.speed);

        if (randomAmbianceTimer <= 0f && PlayerController.instance.isNonPhysics)
        {
            PlayBoatAmbiance();
            randomAmbianceTimer = Random.Range(10f, 25f);
        }
        else
        {
            randomAmbianceTimer -= Time.deltaTime;
        }
    }

    private void PlayBoatAmbiance()
    {
        ambianceWood.clip = sfx_wood_creak;
        ambianceWood.Play();
    }

    private void PlayWavesAmbiance()
    {
        waves.clip = sfx_waves_1;
        waves.Play();
    }

    private void UpdateSailingAmbiance(float speed)
    {
        if (speed < 4f)
        {
            ambianceSailing.clip = sfx_boat_drive_slow;
        }
        else
        {
            ambianceSailing.clip = sfx_boat_drive_fast;
        }

        ambianceSailing.volume = Mathf.Clamp(LerpVolume(ambianceSailing.volume, speed / 7f), 0.1f, 1f);
        waves.volume = Mathf.Clamp(LerpVolume(waves.volume, speed / 7f), 0.2f, 1f);
    }

    private float LerpVolume(float volume, float newVolume)
    {
        return Mathf.Lerp(volume, newVolume, 1f * Time.deltaTime);
    }

    public void PlayAnchorDownSound()
    {
        anchor.clip = sfx_anchor_down;
        anchor.Play();
    }

    public void PlayAnchorUpSound()
    {
        anchor.clip = sfx_anchor_up;
        anchor.Play();
    }

    public void PlayLightSound()
    {
        light.clip = sfx_light;
        light.Play();
    }

    public IEnumerator StartTapeSound()
    {
        tape.clip = sfx_tape_start;
        tape.Play();
        yield return new WaitForSeconds(tape.clip.length);
        tape.clip = sfx_tape_playing;
        tape.loop = true;
        tape.Play();
    }

    public void EndTapeSound()
    {
        tape.loop = false;
    }

    public void EjectTapeSound()
    {
        tape.clip = sfx_tape_end;
        tape.loop = false;
        tape.Play();
    }

    public void PlayGearSound()
    {
        other.clip = sfx_gear_switch;
        other.Play();
    }

    public void PlayTurnSound()
    {
        //TODO: after this is turned into prefab.
    }
}
