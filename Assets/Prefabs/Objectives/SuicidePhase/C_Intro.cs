using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class C_Intro : MonoBehaviour
{
    public GameObject introUI;
    public TextMeshProUGUI displayText;
    public Image backgroundImg;
    public GameObject nextArrow;
    public GameObject assignObjective;

    public string[] cutscene_text;
    private int text_index = 0;
    public float fadeDuration = 1f;

    private bool finishFadeIn = true;
    private bool finishCutscene = false;

    private Coroutine flashArrowCouroutine;

    private void Start()
    {
        StartIntro();
    }

    private void StartIntro()
    {
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);
        displayText.text = cutscene_text[text_index];
        nextArrow.SetActive(false);
        flashArrowCouroutine = StartCoroutine(FlashArrow());
    }

    private void Update()
    {
        if (!finishCutscene)
        {
            PlayerController.instance.LockMovement(true);
            PlayerController.instance.LockCamera(true);
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && finishFadeIn && !finishCutscene)
        {
            if (text_index == cutscene_text.Length - 1)
            {
                StopCoroutine(flashArrowCouroutine);
                nextArrow.SetActive(false);
                StartCoroutine(FadeToWhite());
            }
            else
            {
                text_index++;
                StopCoroutine(flashArrowCouroutine);
                nextArrow.SetActive(false);
                StartCoroutine(FadeOutInNewText());
            }
        }
    }

    private IEnumerator FadeOutInNewText()
    {
        finishFadeIn = false;
        Color opaque = new Color(1f, 1f, 1f, 1f);
        Color transparent = new Color(1f, 1f, 1f, 0f);

        //fade out old text
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            displayText.color = Color.Lerp(opaque, transparent, elapsedTime / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);
        displayText.text = cutscene_text[text_index];

        //fade in new text
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            displayText.color = Color.Lerp(transparent, opaque, elapsedTime / fadeDuration);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);

        finishFadeIn = true;
        flashArrowCouroutine = StartCoroutine(FlashArrow());
    }

    private IEnumerator FlashArrow()
    {
        while (finishFadeIn)
        {
            nextArrow.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            nextArrow.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FadeToWhite()
    {
        Color opaque = new Color(1f, 1f, 1f, 1f);
        Color trasnparent = new Color(1f, 1f, 1f, 0f);

        //fade out into white
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            displayText.color = Color.Lerp(opaque, trasnparent, elapsedTime / fadeDuration);
            backgroundImg.color = Color.Lerp(Color.black, Color.white, elapsedTime / 2f);
            yield return null;
        }
        displayText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.05f);

        finishCutscene = true;
        introUI.SetActive(false);
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);

        ObjectiveManager.instance.AssignObejctive(assignObjective);
        Destroy(gameObject);
    }
}
