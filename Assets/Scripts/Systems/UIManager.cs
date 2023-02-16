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

    public TMP_Text subtitleUI;
    public GameObject inventoryUI;
    public GameObject inventoryItemGrid;
    public GameObject inventoryBackGrid;
    public GameObject recordingUI;
    public GameObject paperUI;
    public TMP_Text paperText;

    void Awake()
    {
        instance = this;
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
        subtitleUI.DOFade(0, subtitleFadeDuration).OnComplete(() =>
        {
            subtitleUI.text = name + ": " + tempSubtitle;
            subtitleUI.DOFade(1, subtitleFadeDuration);
        }
        );
    }

    public void FadeOutSubtitle()
    {
        float subtitleFadeDuration = 0.5f;
        subtitleUI.DOFade(0, subtitleFadeDuration);
    }

    public void ClearSubtitle()
    {
        subtitleUI.DOFade(0, 0.5f).OnComplete(() =>
        {
            subtitleUI.text = "";
        }
        );
    }
}
