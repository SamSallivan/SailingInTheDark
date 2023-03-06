using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Cinemachine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;
    public PlayerController playerMovement;
    public BoatController boatMovement;
    public Rigidbody boatRB;
    public List<PlayableDirector> directors = new List<PlayableDirector>();
    public GameObject allCameras;
    bool playingCutscene = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (!playingCutscene && Input.GetKeyDown(KeyCode.C))
            StartCoroutine(PlayCutscene(directors[0]));
    }

    public IEnumerator PlayCutscene(PlayableDirector cutsceneCamera)
    {
        playingCutscene = true;
        cutsceneCamera.gameObject.SetActive(true);
        playerMovement.enableMovement = false;
        boatMovement.batteryInUse = false;
        boatRB.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

        yield return new WaitForSeconds((float)cutsceneCamera.playableAsset.duration);

        playingCutscene = false;
        cutsceneCamera.gameObject.SetActive(false);
        playerMovement.enableMovement = true;
        boatMovement.batteryInUse = true;
        boatRB.constraints = RigidbodyConstraints.None;
        allCameras.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
