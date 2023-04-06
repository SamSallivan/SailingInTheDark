using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UpgradeData;
using MyBox.Internal;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public bool activated;
    public UpgradeSlot slotPrefab;

    public List<UpgradeOption> upgradeOptionList = new List<UpgradeOption>();
    public int selectedIndex;
    public UpgradeOption selectedOption;

    public ItemData materialItem;
    public int materialCount;
    public TMP_Text materialText;

    //public Slider[] sliderForCosts = new Slider[5];
    public TMP_Text[] textForCosts = new TMP_Text[5];
    public int[] maxCosts = new int[5];

    [HideInInspector] TMP_Text fuelTankLabel;
    public bool[] upgradeUnlocked = new bool[5];

    public BoatController boatController;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //PopulateSlots(new List<UpgradeOption>());
    }

    public void OpenMenu(List<UpgradeOption> list)
    {
        upgradeOptionList = list;
        UIManager.instance.upgradeUI.SetActive(true);
        UIManager.instance.gameplayUI.SetActive(false);
        UIManager.instance.GetComponent<LockMouse>().LockCursor(false);
        PlayerController.instance.LockMovement(true);
        PlayerController.instance.LockCamera(true);
        PopulateSlots();
    }

    public void CloseMenu()
    {
        UIManager.instance.upgradeUI.SetActive(false);
        UIManager.instance.gameplayUI.SetActive(true);
        UIManager.instance.GetComponent<LockMouse>().LockCursor(true);
        PlayerController.instance.LockMovement(false);
        PlayerController.instance.LockCamera(false);
    }

    public void PopulateSlots()
    {
        foreach (Transform slot in UIManager.instance.UpgradeOptionList.transform)
        {
            Destroy(slot.gameObject);
        }
        foreach (UpgradeOption option in upgradeOptionList)
        {
            UpgradeSlot newSlot = Instantiate(slotPrefab, UIManager.instance.UpgradeOptionList.transform);
            newSlot.upgradeOption = option;
            newSlot.UpdateSlot();
        }
    }

    public int CountMaterials(ItemData itemData)
    {
        //Counts material
        int tempMaterialCount = 0;
        foreach (InventoryItem item in InventoryManager.instance.inventoryItemList)
        {
            if (item.data == itemData)
            {
                tempMaterialCount += item.status.amount;
            }
        }
        materialCount = tempMaterialCount;
        return tempMaterialCount;
    }

    public void Update()
    {
    }

    public void Upgrade(UpgradeOption option)
    {
        int i = 0;

        int costLevel = option.currentLevel - 1;
        if (option.upgradeData.costs.Length <= costLevel)
        {
            costLevel = option.upgradeData.costs.Length - 1;
        }
        foreach (MaterialRequired requiredMaterial in option.upgradeData.costs[costLevel].requiredMaterials)
        {
            int materialCount = CountMaterials(requiredMaterial.itemData);
            if (materialCount < requiredMaterial.amount)
            {
                return;
            }
        }

        if (option.currentLevel >= option.upgradeData.maxLevel)
        {
            return;
        }

        foreach (MaterialRequired requiredMaterial in option.upgradeData.costs[costLevel].requiredMaterials)
        {
            CostMaterial(requiredMaterial.itemData, requiredMaterial.amount);
        }

        switch(option.upgradeData.type)
        {
            case UpgradeType.FuelCapacity:
                //boatController.maxWattHour = 100 + (25 * option.currentLevel);
                BoatController.instance.maxWattHour += 25;
                break;

            case UpgradeType.ItemTrade:
                InventoryManager.instance.AddItem(option.upgradeData.itemToTrade, new ItemStatus(option.upgradeData.itemAmount, 1));
                break;

            case UpgradeType.LightIntensity:
                BoatController.instance.lightLeft.lightObject.GetComponent<Light>().intensity = 100;
                BoatController.instance.lightRight.lightObject.GetComponent<Light>().intensity = 100;
                break;

            case UpgradeType.GearUnlock:
                BoatController.instance.helm.currentMaxGear++;
                break;

            case UpgradeType.BoatArmor:
                BoatController.instance.boatArmor += 0.5f;
                break;

        }
        option.currentLevel++;
        PopulateSlots();

    }

    public void CostMaterial(ItemData itemData, int materialCost)
    {
        //materialCount -= x;
        
        int temp = materialCost;

        foreach (InventoryItem item in InventoryManager.instance.inventoryItemList)
        {
            //if (item.data == itemData && item.data.isStackable)
            //{
            if (item.data == itemData)
            {
                while (item.status.amount > 0 && temp > 0)
                {
                    item.status.amount --;
                    temp--;
                    item.slot.amount.text = "" + item.status.amount;

                    //remove from inventory if <= 0
                }

                if (item.status.amount <= 0)
                {
                    InventoryManager.instance.inventoryItemList.Remove(item);
                    Destroy(item.slot.gameObject);
                    if (InventoryManager.instance.equippedItemLeft == item || InventoryManager.instance.equippedItemRight == item || InventoryManager.instance.equippedItemCenter == item)
                    {
                        InventoryManager.instance.UnequipItem(item.data.equipType);
                    }
                }

                if (temp <= 0){
                    return;
                }
                break;
            }
        }
    }

    public void SpendForUpgrade(int type)
    {
        if (!upgradeUnlocked[type] && materialCount > maxCosts[type])
        {
            //CostMaterial(maxCosts[type]);
            upgradeUnlocked[type] = true;

            if (type == 0)
            {
                boatController.maxWattHour = 100 + (25);// * fuelTankLevel);
                //boatController.curWattHour += 0;
                
                upgradeUnlocked[0] = false;
                maxCosts[0] += 20;
            }

            if (type == 2)
            {
                boatController.lightLeft.lightObject.GetComponent<Light>().intensity = 100;
                boatController.lightRight.lightObject.GetComponent<Light>().intensity = 100;
            }

            if (type == 3)
            {
                boatController.boatArmor += 1;
            }

            if (type == 4)
            {
                boatController.helm.currentMaxGear += 1;
            }

            PopulateSlots();
        }
        else
            Debug.Log("can't upgrade");
    }
}
