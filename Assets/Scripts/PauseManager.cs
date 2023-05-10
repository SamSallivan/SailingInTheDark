using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    public Toggle fullScreen;
    public Slider soundLoudness;
    public AudioMixer mixer;

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        fullScreen.isOn = Screen.fullScreen;
        fullScreen.onValueChanged.AddListener(delegate { Screensize(); });

        soundLoudness.value = PlayerPrefs.GetFloat("Volume");
        soundLoudness.onValueChanged.AddListener(delegate { SetLevel(); });
        SetLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !UIManager.instance.pauseUI.activeInHierarchy)
        {
            Time.timeScale = 0;
            UIManager.instance.pauseUI.SetActive(true);
            UIManager.instance.lockMouse.LockCursor(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && UIManager.instance.pauseUI.activeInHierarchy)
        {
            ResumeGame();
            UIManager.instance.lockMouse.LockCursor(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        UIManager.instance.pauseUI.SetActive(false);
        UIManager.instance.lockMouse.LockCursor(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
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
