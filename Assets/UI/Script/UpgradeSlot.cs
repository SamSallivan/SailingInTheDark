using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.UI;
using static UpgradeData;

public class UpgradeSlot : MonoBehaviour
{
    public UpgradeOption upgradeOption;

    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text levelText;
    public List<TMP_Text> costText = new List<TMP_Text>();
    //public Slider progress;

    public void Upgrade()
    {
        UpgradeManager.instance.Upgrade(upgradeOption);
    }

    public void UpdateSlot()
    {
        nameText.text = upgradeOption.upgradeData.name;
        descriptionText.text = upgradeOption.upgradeData.description;

        if (upgradeOption.upgradeData.type == UpgradeType.ItemTrade)
        {
            int stock = upgradeOption.upgradeData.maxLevel - upgradeOption.currentLevel;
            levelText.text = "Stock " + stock;
        }
        else
        {

            switch (upgradeOption.upgradeData.type)
            {
                case UpgradeType.FuelCapacity:
                    upgradeOption.currentLevel = BoatController.instance.fuelLevel;
                    break;
                case UpgradeType.BoatArmor:
                    upgradeOption.currentLevel = BoatController.instance.armorLevel;
                    break;
                case UpgradeType.LightIntensity:
                    upgradeOption.currentLevel = BoatController.instance.lightLevel;
                    break;
                case UpgradeType.GearUnlock:
                    upgradeOption.currentLevel = BoatController.instance.gearLevel;
                    break;
            }

            int stock = upgradeOption.upgradeData.maxLevel - upgradeOption.currentLevel;
            levelText.text = "Stock " + stock;
            levelText.text = "Level " + upgradeOption.currentLevel + " / " + upgradeOption.upgradeData.maxLevel;
        }

        costText[1].gameObject.SetActive(false);
        costText[2].gameObject.SetActive(false);
        if (upgradeOption.currentLevel == upgradeOption.upgradeData.maxLevel)
        {

            if (upgradeOption.upgradeData.maxLevel == 1)
            {
                costText[0].text = "Upgraded";
            }
            else
            {

                if (upgradeOption.upgradeData.type == UpgradeType.ItemTrade)
                {
                    costText[0].text = "Sold Out";
                }
                else
                {
                    costText[0].text = "Upgraded";
                }
            }
        }
        else
        {
            int i = 0;

            int costLevel = upgradeOption.currentLevel - 1;
            if (upgradeOption.upgradeData.costs.Length <= costLevel)
            {
                costLevel = upgradeOption.upgradeData.costs.Length - 1;
            }
            foreach (MaterialRequired requiredMaterial in upgradeOption.upgradeData.costs[costLevel].requiredMaterials)
            {

                int materialCount = UpgradeManager.instance.CountMaterials(requiredMaterial.itemData);
                if (materialCount >= requiredMaterial.amount)
                {
                    costText[i].color = Color.green;
                }
                else
                {
                    costText[i].color = Color.red;
                }

                costText[i].gameObject.SetActive(true);
                costText[i].text = requiredMaterial.itemData.name + " X " + requiredMaterial.amount + " / " +
                                   materialCount;
                i++;
            }
        }
    }
}
