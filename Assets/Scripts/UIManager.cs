using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text interactionName;
    public TMP_Text interactionPrompt;
    public AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
