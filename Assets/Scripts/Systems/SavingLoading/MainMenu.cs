using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MyBox;

public class MainMenu : MonoBehaviour
{
    [Foldout("Base Info", true)]
    public TMP_Text warning;
    public Button newGame;
    public Button continueGame;
    public Button settingsButton;
    public Button exit;
    public GameObject settingsScreen;

    [Foldout("Settings", true)]
    public Toggle fullScreen;
    public Slider soundLoudness;

    SaveData data;

    private void Awake()
    {
        data = ES3.Load<SaveData>("saveData", data);
    }

    private void Start()
    {
        continueGame.interactable = (data != null);
        warning.gameObject.SetActive(data != null);
        settingsScreen.gameObject.SetActive(false);

        newGame.onClick.AddListener(Begin);
        continueGame.onClick.AddListener(Continue);
        settingsButton.onClick.AddListener(ChangeSettings);
        exit.onClick.AddListener(ExitGame);

        fullScreen.isOn = Screen.fullScreen;
        fullScreen.onValueChanged.AddListener(delegate { Screensize(); });
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

    public void ChangeSettings()
    {
        settingsScreen.SetActive(!settingsScreen.activeSelf);
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
}