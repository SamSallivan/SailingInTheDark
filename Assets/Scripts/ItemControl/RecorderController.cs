using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderController : MonoBehaviour
{
    public static bool hasHeardLastTape = false;
    public T_Cutscene cutscene;

    void Start()
    {
        cutscene = GameObject.FindGameObjectWithTag("EndingCutscene").GetComponent<T_Cutscene>();
    }

    void Update()
    {
        if (transform.IsChildOf(PlayerController.instance.transform) && (PlayerController.instance.enableMovement))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (hasHeardLastTape)
                {
                    //trigger cutscene
                    cutscene.StartCutscene();
                }
                else
                {
                    UIManager.instance.Notify("I don't think I should use this yet...");
                }
            }
        }

    }
}
