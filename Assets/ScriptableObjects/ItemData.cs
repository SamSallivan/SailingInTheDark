using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;
using static Interactable;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Standard,
        Tape,
        Fish,
    }
    [Foldout("Base Info", true)]

    public ItemType type;
    public string title;
    public Sprite sprite;
    [TextArea(5, 5)]
    public string description;

    //[Header("Recording")]
    [Foldout("Settings", true)]
    
    public bool isStackable;
    [ConditionalField(nameof(isStackable))]
    public int maxStackAmount = 1;

    public bool isEquippable;
    public enum EquipType
    {
        Left,
        Right,
        Both
    }
    [ConditionalField(nameof(isEquippable))]
    public EquipType equipType;
    public Vector3 equipPosition;
    public Quaternion equipRotation;
    //public GameObject equipObject;

    public bool isDroppable;
    [ConditionalField(nameof(isDroppable))]
    public GameObject dropObject;

    //public bool isExaminable;
    public float examineScale = 1;
    public Quaternion examineRotation;

    /*[ConditionalField(nameof(isExaminable))]
    public bool isReadable;
    [ConditionalField(nameof(isReadable))]
    [TextArea(10, 10)]
    public string readText;*/

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    public string recordingName;

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    public DialogueData recording;

    [ConditionalField(nameof(type), false, ItemType.Fish)]
    public float reelDecreaseCoefficient;
    [ConditionalField(nameof(type), false, ItemType.Fish)]
    public Vector2 nimbbleInterval;
}


[System.Serializable]
public struct ItemStatus
{
    public int amount;
    public int durability;

    public ItemStatus(int amount, int durability){
        this.amount = amount;
        this.durability = durability;
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public ItemStatus status;
    public InventorySlot slot;

    public InventoryItem()
    {
    }

    public InventoryItem(ItemData data, ItemStatus status, InventorySlot slot)
    {
        this.data = data;
        this.status = status;
        this.slot = slot;
    }
}