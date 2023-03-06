using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatComponent: MonoBehaviour
{
    public string name;
    public float wattConsumption;
    public bool componentActivated;

    [AutoProperty] public Interactable componentInteractable;

    public void Start()
    {
        BoatController.instance.components.Add(this);
    }

    public void ShutDown()
    {
        componentInteractable.ShutDown();
    }
}
