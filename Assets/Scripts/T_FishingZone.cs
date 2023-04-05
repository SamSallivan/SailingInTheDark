using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class T_FishingZone : Trigger
{

    public List<FishData> fishList;
    public Vector2 fishAmountInterval;
    public int fishAmount;
    public Vector2 waitTimeInterval;

    public void OnEnable()
    {
        fishAmount = Random.Range((int)fishAmountInterval.x, (int)fishAmountInterval.y);
    }

    public override IEnumerator TriggerEvent()
    {
        if (InventoryManager.instance.equippedItemCenter != null && PlayerController.instance.equippedTransformCenter.GetChild(0).GetComponent<FishingRodController>())
        {
            FishingRodController rod = PlayerController.instance.equippedTransformCenter.GetChild(0).GetComponent<FishingRodController>();
            rod.fishingZone = this;
            rod.waitTime = Random.Range(waitTimeInterval.x, waitTimeInterval.y);
        }
        yield break;
    }

    public IEnumerator Delete()
    {
        Destroy(this.gameObject);
        return null;
    }
}

[System.Serializable]
public struct FishData
{
    public float chance;
    public ItemData fishItemData;
}
