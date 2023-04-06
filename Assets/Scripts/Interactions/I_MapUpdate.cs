using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_MapUpdate : Interactable
{
    public GameObject mapFragment;
    public override IEnumerator InteractionEvent()
    {
        if (mapFragment != null)
        {
            mapFragment.SetActive(false);
            Destroy(this.gameObject);
        }

        yield return null;
    }
}
