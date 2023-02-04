using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Light : Interactable
{
    public GameObject light;
    public override IEnumerator InteractionEvent()
    {
        light.SetActive(!light.activeInHierarchy);
        yield return null;
    }
}
