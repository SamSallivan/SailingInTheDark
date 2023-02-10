using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using static Line;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text interactionName;
    public TMP_Text interactionPrompt;
    public AudioSource audioSource;

    public TMP_Text subtitlePlayer;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeInSubtitle(CharacterName speaker, string tempSubtitle)
    {
        string name = speaker.ToString();
        switch (speaker)
        {
            case CharacterName.Mira:
                name = "<style=\"Blue\">" + name + "</style>";
                break;
            case CharacterName.Arnii:
                name = "<style=\"Yellow\">" + name + "</style>";
                break;
            default:
                name = "";
                break;
        }

        float subtitleFadeDuration = 0.5f;
        subtitlePlayer.DOFade(0, subtitleFadeDuration).OnComplete(() =>
        {
            subtitlePlayer.text = name + ": " + tempSubtitle;
            subtitlePlayer.DOFade(1, subtitleFadeDuration);
        }
        );
    }

    public void FadeOutSubtitle()
    {
        float subtitleFadeDuration = 0.5f;
        subtitlePlayer.DOFade(0, subtitleFadeDuration);
    }

    public void ClearSubtitle()
    {
        subtitlePlayer.DOFade(0, 0.5f).OnComplete(() =>
        {
            subtitlePlayer.text = "";
        }
        );
    }
}
