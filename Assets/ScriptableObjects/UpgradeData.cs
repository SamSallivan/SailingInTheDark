using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using static UpgradeData;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string name;
    public enum UpgradeType
    {
        ItemTrade,
        FuelCapacity,
        LightIntensity,
        LightRange,
        BoatArmor,
        GearUnlock,
    }

    public UpgradeType type;
    public string description;
    public Sprite sprite;

    public int maxLevel;

    public Cost[] costs;

    [System.Serializable]
    public struct Cost
    {
        public List<MaterialRequired> requiredMaterials;
    }

    [System.Serializable]
    public struct MaterialRequired
    {
        public ItemData itemData;
        public int amount;
    }

    [ConditionalField(nameof(type), false, UpgradeType.ItemTrade)]
    public ItemData itemToTrade;
    [ConditionalField(nameof(type), false, UpgradeType.ItemTrade)]
    public int itemAmount;

}


[System.Serializable]
public struct UpgradeStatus
{
    public int currentLevel;

    public UpgradeStatus(int level)
    {
        this.currentLevel = level;
    }
}

[System.Serializable]
public class UpgradeOption
{
    public UpgradeData upgradeData;
    public int currentLevel = 1;
    //public UpgradeStatus status;
    public UpgradeSlot slot;

    public UpgradeOption(UpgradeData data, int level, UpgradeSlot slot)
    {
        this.upgradeData = data;
        this.currentLevel = level;
        this.slot = slot;
    }
}
