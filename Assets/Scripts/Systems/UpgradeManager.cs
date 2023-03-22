using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public GameObject upgradeUI;

    public int material;
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
        fuelTankLabel = GameObject.Find("Fuel Tank Label").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        material = 1000;
        upgradeUI.SetActive(false);
        UpdateTexts();
    }

    void UpdateTexts()
    {
        materialText.text = $"{material} Material";
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
                sliderForCosts[i].value = (float)material / maxCosts[i];
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
        material += x;
        UpdateTexts();
    }

    public void LoseMaterial(int x)
    {
        material -= x;
        UpdateTexts();
    }

    public void SpendForUpgrade(int position)
    {
        if (!upgradeUnlocked[position] && material > maxCosts[position])
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
