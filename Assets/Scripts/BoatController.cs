using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;

public class BoatController : MonoBehaviour
{
    public static BoatController instance;
    public AdvancedShipController boat;
    public float maxWattHour = 1000;
    public float curWattHour = 1000;

    public List<BoatComponent> components = new List<BoatComponent>();
    public float maxActiveComponent = 3;

    public TMP_Text hour;
    public TMP_Text second;
    public TMP_Text percentage;
    public TMP_Text componentCount;

    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        float curWattConsumption = 0;
        float curActiveComponent = 0;

        foreach (BoatComponent component in components)
        {
            if (component.componentActivated)
            {
                curWattHour -= component.wattConsumption / 3600 * Time.deltaTime;
                curWattConsumption += component.wattConsumption;
                curActiveComponent++;
            }
        }

        float estimatedHour = curWattHour / curWattConsumption;
        float estimatedSecond = curWattHour / curWattConsumption * 3600;
        float estimatedPercentage = Mathf.Round(curWattHour / maxWattHour * 100);

        float minutes = Mathf.Floor(estimatedSecond / 60);
        float seconds = Mathf.Floor(estimatedSecond % 60);
        string min = minutes.ToString();
        string sec = seconds.ToString();

        if (minutes < 10)
            min = "0" + minutes.ToString();
        if (seconds < 10)
            sec = "0" + Mathf.RoundToInt(seconds).ToString();

        hour.text = "Estimated Life:";
        second.text = min + ":" + sec;
        percentage.text = estimatedPercentage + "%";
        componentCount.text = curActiveComponent + "/" + maxActiveComponent;

        if (curActiveComponent == 0)
            second.text = "N/A";
        if (curWattHour <= 0 || curActiveComponent > maxActiveComponent)
            ShutDown();
    }

    public void ShutDown()
    {
        foreach (BoatComponent component in components)
        {
            component.ShutDown();
        }
        //sound effect and all
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("take damage");
        curWattHour -= damage;
    }
}
