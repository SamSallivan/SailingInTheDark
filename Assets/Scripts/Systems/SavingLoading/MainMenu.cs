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
    [Foldout("Main Menu", true)]
    public TMP_Text warning;
    public List<Button> settingsToggle;
    public Button newGame;
    public Button continueGame;
    public Button exit;

    [Foldout("Settings", true)]
    public GameObject settingsPage;
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        continueGame.interactable = (data != null);
        warning.gameObject.SetActive(data != null);

        newGame.onClick.AddListener(Begin);
        continueGame.onClick.AddListener(Continue);
        exit.onClick.AddListener(ExitGame);

        settingsPage.SetActive(false);
        for (int i = 0; i < settingsToggle.Count; i++)
            settingsToggle[i].onClick.AddListener(Toggle);

        fullScreen.isOn = Screen.fullScreen;
        fullScreen.onValueChanged.AddListener(delegate { Screensize(); });

        soundLoudness.value = PlayerPrefs.GetFloat("Volume");
        soundLoudness.onValueChanged.AddListener(delegate { SetLevel(); });
        SetLevel();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && settingsPage.activeInHierarchy)
        {
            settingsPage.SetActive(false);
        }
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

    public void Toggle()
    {
        settingsPage.SetActive(!settingsPage.activeSelf);
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