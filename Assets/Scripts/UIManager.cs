using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    public void DisplaySubtitle(string tempSubtitle)
    {
        float subtitleFadeDuration = 0.5f;
        subtitlePlayer.DOFade(0, subtitleFadeDuration).OnComplete(() =>
        {
            subtitlePlayer.text = tempSubtitle;
            subtitlePlayer.DOFade(1, subtitleFadeDuration);
        }
        );
    }
}
