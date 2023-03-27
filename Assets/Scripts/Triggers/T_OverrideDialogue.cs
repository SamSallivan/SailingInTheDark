using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_OverrideDialogue : Trigger
{
    public List<DialogueData> dialogueData;
    public override IEnumerator TriggerEvent()
    {
        int rand = Random.Range(0, dialogueData.Count-1);
        DialogueManager.instance.OverrideDialogue(dialogueData[rand]);
        yield return null;
    }
}
