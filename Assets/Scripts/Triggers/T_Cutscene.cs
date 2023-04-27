using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class T_Cutscene : Trigger
{
    public TimelineAsset cutscene_clip;
    public PlayableDirector director;

    public override void Start()
    {
        base.Start();
        // GameObject mainCamera = Camera.main.gameObject;
        director = Camera.main.gameObject.GetComponent<PlayableDirector>();
    }

    public override IEnumerator TriggerEvent()
    {
        StartCoroutine(StartCutscene());
        yield return null;
    }

    public virtual IEnumerator StartCutscene()
    {
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);

        yield return null;
        EndCutscene();
    }


    public virtual void EndCutscene()
    {
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);
    }
}

