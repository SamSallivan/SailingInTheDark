using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPickup : MonoBehaviour
{
    public bool startCutscene;
    public T_Cutscene cutscene;

    private void OnDestroy()
    {
        // Debug.Log("start cutscene");
        // cutscene.StartCoroutine(cutscene.StartCutscene());
        cutscene.StartCutscene();
        // TriggerCutscene();
    }
}
