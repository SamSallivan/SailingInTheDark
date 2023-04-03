using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_WorkBench : Interactable
{
    public List<UpgradeOption> upgradeOptionList = new List<UpgradeOption>();

    public override IEnumerator InteractionEvent()
    {
        if (!activated)
        {
            activated = true;
            UIManager.instance.upgradeTitle.text = textName;
            UpgradeManager.instance.OpenMenu(upgradeOptionList);
        }
        yield return null;
    }

    public void Update()
    {

        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
            {
                activated = false;
                UpgradeManager.instance.CloseMenu();

            }
        }
    }
}
