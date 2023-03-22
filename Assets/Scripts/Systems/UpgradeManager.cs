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
    public I_Helm helm;

    private void Awake()
    {
        instance = this;
        fuelTankLabel = UpgradeManager.instance.textForCosts[0].transform.parent.GetChild(0).GetComponent<TMP_Text>();
    }

    private void Start()
    {
        materialCount = 1000;
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
                sliderForCosts[i].value = 1;
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
                        helm.currentMaxGear = 2;
                        break;
                }
            }
            else
            {
                textForCosts[i].text = $"{maxCosts[i]} Material";
                sliderForCosts[i].value = (float)materialCount / maxCosts[i];
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

        }
    }

    public void AddMaterial(int x)
    {
        materialCount += x;
        UpdateTexts();
    }

    public void LoseMaterial(int x)
    {
        materialCount -= x;
        UpdateTexts();
    }

    public void SpendForUpgrade(int position)
    {
        if (!upgradeUnlocked[position] && materialCount > maxCosts[position])
        {
            LoseMaterial(maxCosts[position]);
            upgradeUnlocked[position] = true;

            if (position == 0)
            {
                fuelTankLevel++;
                boatController.maxWattHour = 1000 + (200 * fuelTankLevel);
                boatController.curWattHour += 500;

                if (fuelTankLevel < 4)
                {
                    upgradeUnlocked[0] = false;
                    maxCosts[0] += 20;
                }
            }

            UpdateTexts();
        }
        else
            Debug.Log("can't upgrade");
    }
}
