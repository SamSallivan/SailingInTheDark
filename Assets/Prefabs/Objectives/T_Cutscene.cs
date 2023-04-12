using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Cutscene : Trigger
{
    public override IEnumerator TriggerEvent()
    {
        StartCoroutine(StartCutscene());
        yield return null;
    }

    public virtual IEnumerator StartCutscene()
    {
        PlayerController.instance.enableMovement = false;
        yield return null;
        EndCutscene();
    }


    public virtual void EndCutscene()
    {
        PlayerController.instance.enableMovement = true;
    }
}

