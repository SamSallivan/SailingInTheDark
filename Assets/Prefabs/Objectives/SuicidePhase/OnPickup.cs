using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPickup : MonoBehaviour
{
    public bool startCutscene;
    public T_Cutscene cutscene;

    private void Start()
    {
        gameObject.GetComponent<I_InventoryItem>().OnPickUp += OnPickUp;
    }

    private void OnPickUp()
    {
        cutscene.StartCutscene();
    }
}
