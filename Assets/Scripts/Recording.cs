using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordingScriptableObject", menuName = "ScriptableObjects/RecordingScriptableObject", order = 1)]
public class Recording : ScriptableObject
{
    public string nameOfFile;
    public List<string> subtitles = new List<string>();
    public List<AudioClip> audioClips = new List<AudioClip>();

    private void Start()
    {
    }

}
