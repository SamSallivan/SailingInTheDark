using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static Line;
using System;
using MyBox;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Foldout("GamePlay", true)]
    public GameObject gameplayUI;
    public TMP_Text interactionName;
    public TMP_Text interactionPrompt;
    public TMP_Text subtitleUI;

    [Foldout("Inventory", true)]
    public GameObject inventoryUI;
    public GameObject inventoryItemGrid;
    public GameObject inventoryBackGrid;
    
    public TMP_Text detailName;
    public TMP_Text detailDescription;
    public Transform detailObjectPivot;

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
            if (Input.GetKeyDown(KeyCode.Escape))
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
