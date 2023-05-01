using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

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
        StartCutscene();
        yield return null;
    }

    public virtual void StartCutscene()
    {
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);
        StartCoroutine(PlayCutscene());
    }

    public virtual IEnumerator PlayCutscene()
    {
        if (cutscene_clip != null)
        {
            director.playableAsset = cutscene_clip;
            director.Play();
            yield return new WaitForSeconds((float)cutscene_clip.duration);
            director.playableAsset = null;
        }

        yield return null;
        EndCutscene();
    }

    // public virtual IEnumerator StartCutscene1()
    // {
    //     PlayerController.instance.LockMovement(true);
    //     PlayerController.instance.LockCamera(true);

    //     if (cutscene_clip != null)
    //     {
    //         director.playableAsset = cutscene_clip;
    //         director.Play();
    //         yield return new WaitForSeconds((float)cutscene_clip.duration);
    //         director.playableAsset = null;
    //     }

    //     yield return null;
    //     EndCutscene();
    // }


    public virtual void EndCutscene()
    {
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);
    }

    // TriggerCutscene()
}

