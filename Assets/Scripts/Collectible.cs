using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string description;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UI.instance.AddToInventory(this);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UI.instance.RemoveFromInventory(this);
        }
    }
}
