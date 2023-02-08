using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("Interaction")]
    public TMP_Text interactionName;
    public TMP_Text interactionPrompt;

    [Header("Inventory")]
    public InventorySlot InventorySlotPrefab;
    public List<Collectible> CollectiblesInInventory = new List<Collectible>();
    public Transform UIInventory;

    void Awake()
    {
        instance = this;
        UIInventory.localPosition = new Vector3(-1000, 0, 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            UIInventory.localPosition = new Vector3(0, 0, 0);
        if (Input.GetKeyUp(KeyCode.Tab))
            UIInventory.localPosition = new Vector3(-1000, 0, 0);
    }

    public void AddToInventory(Collectible newCollectible)
    {
        CollectiblesInInventory.Add(newCollectible);
        InventorySlot newSlot = Instantiate(InventorySlotPrefab, UIInventory);
        newSlot.Setup(newCollectible);
    }

    public void RemoveFromInventory(Collectible newCollectible)
    {
        if (CollectiblesInInventory.Remove(newCollectible))
        {
            Destroy(GameObject.Find(newCollectible.name + $" Slot").gameObject);
        }
    }
}
