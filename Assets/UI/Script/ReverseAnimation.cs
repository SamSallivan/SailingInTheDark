using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReverseAnimation : MonoBehaviour
{
    void OnEnable()
    {
        //GetComponent<Animation>()["Any State"].normalizedTime = 1;
        foreach (AnimationState state in GetComponent<Animation>())
        {
            //state.normalizedTime = 1;
        }
    }
}
