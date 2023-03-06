using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<Line> lines = new List<Line>();
    public List<string> subtitles = new List<string>();
    public List<AudioClip> audioClips = new List<AudioClip>();

    public void Awake()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            subtitles.Add(lines[i].subtitle);
            audioClips.Add(lines[i].audioClip);
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
