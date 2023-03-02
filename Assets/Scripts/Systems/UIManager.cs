using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static Line;
using System;
using MyBox;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Foldout("GamePlay", true)]
    public GameObject gameplayUI;
    public TMP_Text interactionName;
    public TMP_Text interactionPrompt;
    public TMP_Text radioSubtitleUI;
    public TMP_Text dialogueSubtitleUI;
    public Animation interactionPromptAnimation;

    [Foldout("Inventory", true)]
    public GameObject inventoryUI;
    public GameObject inventoryItemGrid;
    public GameObject inventoryBackGrid;
    public Animation inventoryAnimation;
    
    public TMP_Text detailName;
    public TMP_Text detailDescription;
    public Transform detailObjectPivot;
    public bool detailObjectInBound;
    public bool detailObjectDrag;

    [Foldout("Examine", true)]
    public GameObject examineUI;
    public TMP_Text examineText;
    public Image examineImage;
    public Transform examinePivot;

    [Foldout("Recording", true)]
    public GameObject recordingUI;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (examineUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                Unexamine();
            }
        }
    }

    public void Examine(string text)
    {
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);

        examineUI.SetActive(true);
        gameplayUI.SetActive(false);
        examineText.text = text;
    }
    public void Unexamine()
    {
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        examineUI.SetActive(false);
        gameplayUI.SetActive(true);
        examineText.text = "";
    }

    public enum SubtitleType
    {
        Radio,
        Dialogue
    }

    public void FadeInSubtitle(CharacterName speaker, string tempSubtitle, SubtitleType type)
    {
        TMP_Text subtitleUI;
        switch (type)
        {
            case SubtitleType.Radio:
                subtitleUI = radioSubtitleUI;
                break;

            case SubtitleType.Dialogue:
                subtitleUI = dialogueSubtitleUI;
                break;

            default:
                subtitleUI = dialogueSubtitleUI;
                break;
        }

        subtitleUI.gameObject.SetActive(true);
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

        switch (type)
        {
            case SubtitleType.Radio:
                name = "(Recording) " + name;
                break;
        }

        float subtitleFadeDuration = 0.25f;
        subtitleUI.DOFade(0, subtitleFadeDuration).OnComplete(() =>
        {
            subtitleUI.text = name + ": " + tempSubtitle;

            int line = 1 + subtitleUI.text.Length / 72;
            subtitleUI.rectTransform.SetHeight(Math.Clamp(50 * line, 50, 200));

            subtitleUI.DOFade(1, subtitleFadeDuration);
        }
        );
    }

    public void FadeOutSubtitle(SubtitleType type)
    {
        TMP_Text subtitleUI;
        switch (type)
        {
            case SubtitleType.Radio:
                subtitleUI = radioSubtitleUI;
                break;

            case SubtitleType.Dialogue:
                subtitleUI = dialogueSubtitleUI;
                break;

            default:
                subtitleUI = dialogueSubtitleUI;
                break;
        }

        float subtitleFadeDuration = 0.25f;
        subtitleUI.DOFade(0, subtitleFadeDuration).OnComplete(() =>
        {
            subtitleUI.gameObject.SetActive(false);
        }
        );
    }

    public void ClearSubtitle(SubtitleType type)
    {
        TMP_Text subtitleUI;
        switch (type)
        {
            case SubtitleType.Radio:
                subtitleUI = radioSubtitleUI;
                break;

            case SubtitleType.Dialogue:
                subtitleUI = dialogueSubtitleUI;
                break;

            default:
                subtitleUI = dialogueSubtitleUI;
                break;
        }

        subtitleUI.DOFade(0, 0.5f).OnComplete(() =>
        {
            subtitleUI.text = "";
            subtitleUI.gameObject.SetActive(false);
        }
        );
    }
}
