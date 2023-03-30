using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class I_WorkBench : Interactable
{
    public ItemData materialItem;
    public int materialCount;

    public bool[] upgradeUnlocked = new bool[5];
    public int[] upgradeCost = new int[5];
    public int fuelTankLevel = 0;

    public override IEnumerator InteractionEvent()
    {
        if (!activated)
        {
            activated = true;
            UIManager.instance.upgradeUI.SetActive(true);
            UIManager.instance.gameplayUI.SetActive(false);
            UIManager.instance.GetComponent<LockMouse>().LockCursor(false);
            PlayerController.instance.LockMovement(true);
            PlayerController.instance.LockCamera(true);

            UpdateTexts();
        }
        yield return null;
    }

    public void Update()
    {

        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                activated = false;
                UIManager.instance.upgradeUI.SetActive(false);
                UIManager.instance.gameplayUI.SetActive(true);
                UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
                PlayerController.instance.LockMovement(false);
                PlayerController.instance.LockCamera(false);

            }
        }
    }

    void UpdateTexts()
    {
        int tempMaterialCount = 0;
        foreach (InventoryItem item in InventoryManager.instance.inventoryItemList)
        {
            if (item.data == materialItem)
            {
                tempMaterialCount += item.status.amount;
            }
        }
        materialCount = tempMaterialCount;

        UIManager.instance.materialCount.text = $"Material: {materialCount} ";
        //fuelTankLabel.text = $"Fuel Tank LV{fuelTankLevel + 1}";

        for (int i = 0; i < 5; i++)
        {
            /*if (upgradeUnlocked[i])
            {
                //sliderForCosts[i].value = 1;
                switch (i)
                {
                    case 0:
                        textForCosts[i].text = $"Maxed Out!";
                        break;
                    case 1:
                        textForCosts[i].text = $"Unlocked!";
                        break;
                    case 2:
                        textForCosts[i].text = $"Unlocked!";
                        break;
                    case 3:
                        textForCosts[i].text = $"Unlocked!";
                        break;
                    case 4:
                        textForCosts[i].text = $"Unlocked!";
                        break;
                }
            }
            else
            {
                textForCosts[i].text = $"{upgradeCost[i]} Material";
                //sliderForCosts[i].value = (float)materialCount / maxCosts[i];
            }*/
        }
    }
    public void Upgrade(int type)
    {
        if (!upgradeUnlocked[type] && materialCount > upgradeCost[type])
        {
            CostMaterial(upgradeCost[type]);
            upgradeUnlocked[type] = true;

            if (type == 0)
            {
                fuelTankLevel++;
                BoatController.instance.maxWattHour = 100 + (25 * fuelTankLevel);
                //boatController.curWattHour += 0;

                if (fuelTankLevel < 4)
                {
                    upgradeUnlocked[0] = false;
                    upgradeCost[0] += 20;
                }
            }

            if (type == 3)
            {
                BoatController.instance.boatArmor += 1;
            }

            if (type == 4)
            {
                BoatController.instance.helm.currentMaxGear += 1;
            }

            UpdateTexts();
        }
        else
            Debug.Log("can't upgrade");
    }
    public void CostMaterial(int materialCost)
    {
        //materialCount -= x;
        foreach (InventoryItem item in InventoryManager.instance.inventoryItemList)
        {
            if (item.data == materialItem)
            {
                item.status.amount -= materialCost;
                item.slot.amount.text = "" + item.status.amount;
                UpdateTexts();
                break;
            }
        }
    }
}
