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
        Upgrade,
    }
    [Foldout("Base Info", true)]

    public ItemType type;
    public string title;
    public Sprite sprite;
    [TextArea(5, 5)]
    public string description;

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    public string recordingName;

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    public DialogueData recording;

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
    public GameObject equipObject;
    public Quaternion equipRotation;
    public Vector3 equipPosition;

    public bool isDroppable;
    [ConditionalField(nameof(isDroppable))]
    public GameObject dropObject;

    public bool isExaminable;
    public float examineScale = 1;
    public Quaternion examineRotation;

    [ConditionalField(nameof(isExaminable))]
    public bool isReadable;
    [ConditionalField(nameof(isReadable))]
    [TextArea(10, 10)]
    public string readText;
}


[System.Serializable]
public struct ItemStatus
{
    public int amount;
    public int durability;
}