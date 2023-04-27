using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MyBox;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public TMP_Text warning;
    public Button newGame;
    public Button continueGame;
    public Button exit;

    public Toggle fullScreen;
    public Slider soundLoudness;
    public AudioMixer mixer;

    SaveData data;

    private void Awake()
    {
        data = ES3.Load<SaveData>("saveData", data);
    }

    private void Start()
    {
        continueGame.interactable = (data != null);
        warning.gameObject.SetActive(data != null);

        newGame.onClick.AddListener(Begin);
        continueGame.onClick.AddListener(Continue);
        exit.onClick.AddListener(ExitGame);

        fullScreen.isOn = Screen.fullScreen;
        fullScreen.onValueChanged.AddListener(delegate { Screensize(); });

        soundLoudness.value = PlayerPrefs.GetFloat("Volume");
        soundLoudness.onValueChanged.AddListener(delegate { SetLevel(); });
        SetLevel();
    }

    public void Begin()
    {
        SaveLoader.DeleteSaveData();
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

    public void Screensize()
    {
        Screen.fullScreenMode = (fullScreen.isOn) ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    public void SetLevel()
    {
        mixer.SetFloat("Volume", (Mathf.Log10(soundLoudness.value) * 20));
        PlayerPrefs.SetFloat("Volume", soundLoudness.value);
    }
}