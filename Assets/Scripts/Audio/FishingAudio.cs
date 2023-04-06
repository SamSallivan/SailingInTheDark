using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class FishingAudio : MonoBehaviour
{
    [Foldout("AudioSources", true)]
    public AudioSource rod; //ready rod, throw, (1s) throw reel, land lure, pull
    public AudioSource ambiance; //ambiant splashing
    public AudioSource splashing; //fish fighting
    public AudioSource minigame; //bite, increase bar, fail/success
    public AudioSource reeling; //reeling

    [Foldout("AudioClips", true)]
    public AudioClip sfx_bite; //once, then start fish fighting
    public AudioClip sfx_splash_success; //once
    public AudioClip sfx_bubbles_fail; //fail A
    public AudioClip sfx_splash_fail; //fail B
    public AudioClip sfx_fish_fighting; //loop
    public AudioClip sfx_lure_land; //once
    public AudioClip sfx_reeling; //loop
    public AudioClip sfx_splash_ambiance; //starts, loop on timer, ends
    public AudioClip sfx_throw; //once
    public AudioClip sfx_player_pull; //once

    public AudioClip sfx_ready_rod; //once
    public AudioClip sfx_throw_reel; //once
    public AudioClip sfx_fake_bite; //once
    public AudioClip sfx_early_pull_bubble; //once
    public AudioClip sfx_early_pull_splash; //once

    private float ambianceTimer;
    private bool isFishing = false;
    private bool isPulling = false;

    private void Start()
    {
        ambianceTimer = Random.Range(2f, 7f);
    }

    private void Update()
    {
        if (isFishing)
        {
            if (ambianceTimer <= 0f)
            {
                ambiance.clip = sfx_splash_ambiance;
                ambiance.Play();
                ambianceTimer = Random.Range(2f, 7f);
            }
            else
            {
                ambianceTimer -= Time.deltaTime;
            }
        }
    }

    public void StartFishingAmbiance()
    {
        isFishing = true;
    }

    public void EndFishingAmbiance()
    {
        isFishing = false;
    }

    public void PlayReadyRod()
    {
        rod.clip = sfx_ready_rod;
        rod.Play();
    }

    public void PlayFakeBite()
    {
        minigame.clip = sfx_fake_bite;
        minigame.Play();
    }

    public IEnumerator PlayEarlyPull()
    {
        StartCoroutine(EndReeling());
        StartCoroutine(EndSplashing());

        rod.clip = sfx_player_pull;
        rod.Play();
        minigame.clip = sfx_early_pull_bubble;
        minigame.Play();
        yield return new WaitForSeconds(0.05f);
        minigame.clip = sfx_early_pull_splash;
        minigame.Play();
    }

    public IEnumerator PlayThrowLure()
    {
        rod.clip = sfx_throw;
        rod.Play();
        yield return new WaitForSeconds(0.5f);
        reeling.volume = 1f;
        reeling.clip = sfx_throw_reel;
        reeling.loop = false;
        reeling.Play();
    }

    public void PlayLureLand()
    {
        rod.clip = sfx_lure_land;
        rod.Play();
    }

    public IEnumerator PlayPlayerPull()
    {
        if (!isPulling)
        {
            isPulling = true;
            rod.clip = sfx_player_pull;
            rod.Play();
            yield return new WaitForSeconds(0.02f);
            isPulling = false;
        }
    }

    public IEnumerator PlayRealBite()
    {
        minigame.clip = sfx_bite;
        minigame.Play();
        yield return new WaitForSeconds(0.3f);
        splashing.volume = 1f;
        splashing.clip = sfx_fish_fighting;
        splashing.loop = true;
        splashing.Play();
    }

    public IEnumerator PlayFishingFailure()
    {
        StartCoroutine(EndReeling());

        while (splashing.volume > 0f)
        {
            splashing.volume -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
        splashing.volume = 0f;

        splashing.loop = false;
        splashing.Stop();

        splashing.volume = 1f;
        splashing.clip = sfx_splash_fail;
        splashing.loop = false;
        splashing.Play();

        minigame.clip = sfx_bubbles_fail;
        minigame.Play();
        yield return new WaitForSeconds(0.05f);
    }

    public void PlayFishingSuccess()
    {
        StartCoroutine(EndReeling());
        StartCoroutine(EndSplashing());

        minigame.clip = sfx_splash_success;
        minigame.Play();
    }

    public void StartReeling()
    {
        reeling.volume = 1f;
        reeling.clip = sfx_reeling;
        reeling.loop = true;
        reeling.Play();
    }

    private IEnumerator EndReeling()
    {
        while (reeling.volume > 0f)
        {
            reeling.volume -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        reeling.volume = 0f;

        reeling.loop = false;
        reeling.Stop();
    }

    private IEnumerator EndSplashing()
    {
        while (splashing.volume > 0f)
        {
            splashing.volume -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        splashing.volume = 0f;

        splashing.loop = false;
        splashing.Stop();
    }
}
