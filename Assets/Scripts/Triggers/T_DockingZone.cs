using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_DockingZone : Trigger
{
    public Transform dockingPos;
    public Transform anchorPos;
    public I_Anchor anchor;
    public override IEnumerator TriggerEvent()
    {
        if (!anchor.activated)
        {
            anchor.textPrompt = "Dock";
        }
        anchor.dockable = true;
        anchor.dockingZone = this;
        yield return null;
    }

    public void OnTriggerStay(Collider other)
    {
        if (!anchor.activated)
        {
            anchor.textPrompt = "Dock";
        }
        anchor.dockable = true;
        anchor.dockingZone = this;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!anchor.activated)
        {
            anchor.textPrompt = "Drop";
        }
        anchor.dockable = false;
        anchor.dockingZone = null;

    }
}
