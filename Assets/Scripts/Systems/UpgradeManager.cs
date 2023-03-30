using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public GameObject upgradeUI;

    public ItemData materialItem;
    public int materialCount;
    public TMP_Text materialText;

    public Slider[] sliderForCosts = new Slider[5];
    public TMP_Text[] textForCosts = new TMP_Text[5];
    public int[] maxCosts = new int[5];

    [HideInInspector] public int fuelTankLevel = 0;
    [HideInInspector] TMP_Text fuelTankLabel;
    public bool[] upgradeUnlocked = new bool[5];

    public BoatController boatController;


    private void Awake()
    {
        instance = this;
        fuelTankLabel = UpgradeManager.instance.textForCosts[0].transform.parent.GetChild(0).GetComponent<TMP_Text>();
    }

    private void Start()
    {
        upgradeUI.SetActive(false);
        UpdateTexts();
    }

    void UpdateTexts()
    {
        int tempMaterialCount = 0;
        foreach(InventoryItem item in InventoryManager.instance.inventoryItemList)
        {
            if (item.data == materialItem)
            {
                tempMaterialCount += item.status.amount;
            }
        }
        materialCount = tempMaterialCount;


        materialText.text = $"{materialCount} Material";
        fuelTankLabel.text = $"Fuel Tank LV{fuelTankLevel + 1}";

        for (int i = 0; i<5; i++)
        {
            if (upgradeUnlocked[i])
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
                textForCosts[i].text = $"{maxCosts[i]} Material";
                //sliderForCosts[i].value = (float)materialCount / maxCosts[i];
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateTexts();
            upgradeUI.SetActive(!upgradeUI.activeSelf);
            UIManager.instance.GetComponent<LockMouse>().LockCursor(!upgradeUI.activeSelf);

            if (upgradeUI.activeInHierarchy)
            {
                PlayerController.instance.LockMovement(true);
                PlayerController.instance.LockCamera(true);
            }
            else
            {
                PlayerController.instance.LockMovement(false);
                PlayerController.instance.LockCamera(false);
            }

        }
    }

    public void LoseMaterial(int materialCost)
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

    public void SpendForUpgrade(int type)
    {
        if (!upgradeUnlocked[type] && materialCount > maxCosts[type])
        {
            LoseMaterial(maxCosts[type]);
            upgradeUnlocked[type] = true;

            if (type == 0)
            {
                fuelTankLevel++;
                boatController.maxWattHour = 100 + (25 * fuelTankLevel);
                //boatController.curWattHour += 0;

                if (fuelTankLevel < 4)
                {
                    upgradeUnlocked[0] = false;
                    maxCosts[0] += 20;
                }
            }

            if (type == 3)
            {
                boatController.boatArmor += 1;
            }

            if (type == 4)
            {
                boatController.helm.currentMaxGear += 1;
            }

            UpdateTexts();
        }
        else
            Debug.Log("can't upgrade");
    }
}
