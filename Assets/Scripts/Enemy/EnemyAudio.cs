using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class EnemyAudio : MonoBehaviour
{
    public EnemyMovement enemyMovement;

    [Foldout("AudioSources", true)]
    public AudioSource swimming;
    public AudioSource splashing;
    public AudioSource voice;
    public AudioSource ambiance;
    public AudioSource other;

    [Foldout("AudioClips", true)]
    public AudioClip sfx_death_splash;
    public AudioClip sfx_ambiance;
    public AudioClip sfx_swimming;
    public AudioClip sfx_hit_metal;
    public AudioClip sfx_hit_wood;
    public AudioClip sfx_scream_spawn;
    public AudioClip sfx_scream_attack;
    public AudioClip sfx_scream_death;
    public AudioClip sfx_scream_hurt;
    public AudioClip sfx_attack_splash;

    // [Foldout("Fields", true)]
    public float swimming_volume = 0.1f;
    private float randomAmbianceTimer = 0f;

    private void Start()
    {
        voice.volume = 0.2f;
        ambiance.volume = 0.2f;
        other.volume = 0.2f;

        voice.clip = sfx_scream_spawn;
        voice.Play();

        swimming.clip = sfx_swimming;
        swimming.Play();
        swimming.volume = swimming_volume;
        StartCoroutine(IncreaseVolume());

        randomAmbianceTimer = Random.Range(10f, 25f);

    }

    private void Update()
    {
        // swimming_volume = swimming_volume + 0.01f < 1f ? swimming_volume + 0.2f : 1f;
        swimming.volume = swimming_volume;

        if (randomAmbianceTimer <= 0f)
        {
            PlayAmbiance();
            randomAmbianceTimer = Random.Range(10f, 25f);
        }
        else
        {
            randomAmbianceTimer -= Time.deltaTime;
        }

    }

    private IEnumerator IncreaseVolume()
    {
        while (swimming_volume < 1f)
        {
            swimming_volume += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        swimming_volume = 1f;
    }
    private void PlayAmbiance()
    {
        ambiance.clip = sfx_ambiance;
        ambiance.Play();
    }

    public IEnumerator PlayDeathSound()
    {
        voice.clip = sfx_scream_death;
        voice.Play();
        splashing.clip = sfx_death_splash;
        splashing.Play();
        while (swimming_volume > 0)
        {
            swimming_volume -= 0.01f;
            swimming.volume = swimming_volume;
            // Debug.Log(swimming.volume);
            yield return new WaitForSeconds(0.1f);
        }
        enemyMovement.DestroySelf();
    }

    public IEnumerator PlayAttackSound()
    {
        // Debug.Log("attack sound");
        voice.clip = sfx_scream_attack;
        voice.Play();
        yield return new WaitForSeconds(voice.clip.length);
        splashing.clip = sfx_attack_splash;
        splashing.Play();
        yield return new WaitForSeconds(0.01f);
        other.clip = Random.value < 0.5f ? sfx_hit_wood : sfx_hit_metal; //hit wood is really loud
        if (other.clip == sfx_hit_metal)
        {
            other.volume = 0.5f;
        }
        // other.clip = sfx_hit_wood;
        other.Play();
        yield return new WaitForSeconds(other.clip.length);
        other.volume = 0.2f;
    }

    public void PlayHurtSound()
    {
        voice.clip = sfx_scream_hurt;
        voice.Play();
    }
}
