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
        Paper
    }
    public ItemType type;
    public string title;
    public string description;
    public Sprite sprite;
    public GameObject dropObject;
    public GameObject heldObject;

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    //[Header("Recording")]
    public DialogueData recording;

    [ConditionalField(nameof(type), false, ItemType.Tape)]
    public string recordingName;

    [Serializable]
    public class Toggles
    {
        [Tooltip("Item can be stored in one item depending on the MaxStackAmount.")]
        public bool isStackable;
        [Tooltip("Item can be used from inventory.")]
        public bool isUsable;
        [Tooltip("Item can be dropped to the ground.")]
        public bool isDroppable;

        //[Tooltip("Item can be examined from the inventory.")]
        //public bool canExamine;
    }
    public Toggles itemToggles = new Toggles();

    [Serializable]
    public sealed class Settings
    {
        [Tooltip("How many items can be stored in one item.")]
        public int maxStackAmount = 1;
        //[Tooltip("Default Rotation of the examined item.")]
        //public Vector3 examineRotation;
    }
    public Settings itemSettings = new Settings();
}


[System.Serializable]
public struct ItemStatus
{
    public int amount;
    public int durability;
    [TextArea(15, 20)]
    public string text;
}