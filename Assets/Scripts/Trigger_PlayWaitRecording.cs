using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_PlayWaitRecording : Trigger
{
    /*Trigger Defination: 
     * when player enter this trigger, all the waited audio will be played.
     * this is usually attahed to a ship, or somewhere close to the radio player.
     */

    public override IEnumerator TriggerEvent()
    {
        DialogueManager.instance.PlayWaitlistDialogue();
        yield return null;
    }
}
