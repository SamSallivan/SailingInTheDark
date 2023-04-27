using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_death : T_Cutscene
{
    public override IEnumerator StartCutscene()
    {
        // PlayerController.instance.enableMovement = false;
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);

        director.playableAsset = cutscene_clip;
        director.Play();
        yield return new WaitForSeconds((float)cutscene_clip.duration);
        EndCutscene();
    }
}
