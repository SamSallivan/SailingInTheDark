using NWH.DWP2.ShipController;
using System.Collections;
using System.Collections.Generic;
using Crest;
using UnityEngine;
using MyBox;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

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
    public BoatAudio boatAudio;
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

    [Foldout("Upgrades", true)]
    public int fuelLevel = 1;
    public int armorLevel = 1;
    public int lightLevel = 1;
    public int gearLevel = 1;


    [Foldout("TMPs", true)]
    public TMP_Text percentageText;
    public TMP_Text usageText;
    public TMP_Text timerText;
    public TMP_Text componentCountText;
    public TMP_Text speedText;
    public Image fuelImage;
    public Image fuelBackImage;
    public Image gearImage;
    public Color Green, Red, Yellow;
    public TMP_Text Gear3, Gear4;

    public void Awake()
    {
        instance = this;

        curWattHour = maxWattHour;
        BoatComponent[] temp = FindObjectsOfType<BoatComponent>(true);
        shakeSource = GetComponent<CinemachineImpulseSource>();

    }
    private void OnEnable()
    {
        switch (helm.currentMaxGear)
        {
            case 3:
                Gear3.gameObject.SetActive(true);
                break;

            case 4:
                Gear3.gameObject.SetActive(true);
                Gear4.gameObject.SetActive(true);
                break;
        }
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

                float consumption = component.wattConsumption;
                if (component.name == "Helm")
                {
                    consumption = component.wattConsumption * Mathf.Abs(helm.currentGear / helm.totalGearNumber);
                }
                curWattConsumption += consumption;

                if (!ignoreConsumption && !(anchor.dockable && anchor.activated))
                {
                    curWattHour -= consumption / 3600 * Time.deltaTime;
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

        percentageText.text = percentage.ToString();
        fuelImage.fillAmount = percentage / 200;
        fuelBackImage.fillAmount =  maxWattHour / 200; ;
        usageText.text = Mathf.Round(curWattConsumption / refWattHour / 3600 * 100 * 100) * 0.01f + "% / sec";
        timerText.text = min + ":" + sec;
        componentCountText.text = curActiveComponent + "/" + maxActiveComponent;
        speedText.text = Mathf.Round(BoatController.instance.boat.Speed) + "km/h";
        gearImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(610 + helm.currentGear * 75,11.5f,0);
        switch (helm.currentGear)
        {
            case -1:
                gearImage.color = Yellow;
                break;
            case 0:
                gearImage.color = Red;
                break;
            default:
                gearImage.color = Green;
                break;
        }


        if (curActiveComponent == 0 || ignoreConsumption || (anchor.dockable && anchor.activated))
        {
            timerText.text = "N/A";
        }

        if (anchor.dockable && anchor.activated)
        {
            curWattHour = Mathf.Lerp(curWattHour, maxWattHour, Time.fixedDeltaTime / 5);
            if (curWattHour < maxWattHour)
            {
                percentageText.text += " (Charging)";
            }
            else
            {
                percentageText.text += " (Fully Charged)";
            }
            //is charging the boat
        }

        if (!SaveManager.instance.isGameOver && curWattHour <= 0) //|| curActiveComponent > maxActiveComponent)
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
        curWattHour = (curWattHour - damage / (1 + boatArmor) < 0) ? 0 : curWattHour - damage / (1 + boatArmor);

        if (curWattHour <= 0 && !UIManager.instance.gameOverUI.activeInHierarchy)
        {
            SaveManager.instance.Die("Your boat was destroyed.");
        }
        //sound effect and all

        //CameraShake
        shakeSource.GenerateImpulse();
    }
}
