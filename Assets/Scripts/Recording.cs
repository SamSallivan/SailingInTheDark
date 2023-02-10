using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordingScriptableObject", menuName = "ScriptableObjects/RecordingScriptableObject", order = 1)]
public class Recording : ScriptableObject
{
    public List<Line> lines = new List<Line>();
    public List<string> subtitles = new List<string>();
    public List<AudioClip> audioClips = new List<AudioClip>();

    public void Awake()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            subtitles[i] = lines[i].subtitle;
            audioClips[i] = lines[i].audioClip;
        }
    }
}
[System.Serializable]
public struct Line
{
    public enum CharacterName
    {
        Mira,
        Arnii
    }

    public float intervalBefore;
    public CharacterName speaker;
    public string subtitle;
    public AudioClip audioClip;
    public float intervalAfter;
}
