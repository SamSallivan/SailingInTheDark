using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using Crest;
using UnityEngine;
using MyBox;
using TMPro;
using Cinemachine;

public class BoatController : MonoBehaviour
{
    [Foldout("References", true)]
    public static BoatController instance;
    public AdvancedShipController boat;
    public I_Helm helm;
    public I_Anchor anchor;
    public I_Engine engine;
    public I_BoatLight lightLeft;
    public I_BoatLight lightRight;
    public CinemachineImpulseSource shakeSource;
    [ReadOnly]
    public List<BoatComponent> components = new List<BoatComponent>();
    public SphereWaterInteraction waterSphere;

    [Foldout("Settings", true)]
    public float refWattHour = 100;
    public float maxWattHour = 100;
    [ReadOnly]
    public float curWattHour = 100;
    [ReadOnly]
    public float maxActiveComponent = 5;

    public bool ignoreConsumption = false;

    public float boatArmor = 0;


    [Foldout("TMPs", true)]
    public TMP_Text percentageText;
    public TMP_Text usageText;
    public TMP_Text timerText;
    public TMP_Text componentCountText;

    public void Awake()
    {
        instance = this;

        curWattHour = maxWattHour;
        BoatComponent[] temp = FindObjectsOfType<BoatComponent>(true);
        shakeSource = GetComponent<CinemachineImpulseSource>();
    }

    public void FixedUpdate()
    {
        float curWattConsumption = 0;
        float curActiveComponent = 0;

        foreach (BoatComponent component in components)
        {
            if (component.componentActivated)
            {
                curActiveComponent++;
                curWattConsumption += component.wattConsumption;

                if (!ignoreConsumption && !(anchor.dockable && anchor.activated))
                {
                    curWattHour -= component.wattConsumption / 3600 * Time.deltaTime;
                }

            }
        }


        float percentage = Mathf.Round(curWattHour / refWattHour * 100);
        float estimatedHour = curWattHour / curWattConsumption;
        float estimatedSecond = curWattHour / curWattConsumption * 3600;

        float minutes = Mathf.Floor(estimatedSecond / 60);
        float seconds = Mathf.Floor(estimatedSecond % 60);
        string min = minutes.ToString();
        string sec = seconds.ToString();

        if (minutes < 10)
            min = "0" + minutes.ToString();
        if (seconds < 10)
            sec = "0" + Mathf.RoundToInt(seconds).ToString();

        percentageText.text = percentage + "%";
        usageText.text = Mathf.Round(curWattConsumption / refWattHour / 3600 * 100 * 100) * 0.01f + "% / sec";
        timerText.text = min + ":" + sec;
        componentCountText.text = curActiveComponent + "/" + maxActiveComponent;

        if (curActiveComponent == 0 || ignoreConsumption || (anchor.dockable && anchor.activated))
        {
            timerText.text = "N/A";
        }

        if (anchor.dockable && anchor.activated)
        {
            curWattHour = Mathf.Lerp(curWattHour, maxWattHour, Time.fixedDeltaTime / 5);
            percentageText.text += " (Charging)";
            //is charging the boat
        }

        if (SaveManager.instance.alive && curWattHour <= 0) //|| curActiveComponent > maxActiveComponent)
        {
            //ShutDown();
            SaveManager.instance.Die("Your boat ran out of energy.");
        }
    }

    public void ShutDown()
    {
        foreach (BoatComponent component in components)
        {
            component.ShutDown();
        }
    }

    public void TakeDamage(int damage)
    {
        // Debug.Log("take damage");
        curWattHour = (curWattHour - damage/(1+boatArmor) < 0) ? 0 : curWattHour - damage / (1 + boatArmor);

        if (curWattHour <= 0 && !UIManager.instance.gameOverUI.activeInHierarchy)
        {
            SaveManager.instance.Die("Your boat ran out of energy.");
        }
        //sound effect and all

        //CameraShake
        shakeSource.GenerateImpulse();
    }
}
