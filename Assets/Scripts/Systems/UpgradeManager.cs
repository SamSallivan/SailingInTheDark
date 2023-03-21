using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public GameObject upgradeUI;
    public int material;
    public TMP_Text materialText;

    public Slider[] sliderForCosts = new Slider[5];
    public TMP_Text[] textForCosts = new TMP_Text[5];
    public int[] maxCosts = new int[5];
    public int[] listOfCosts = new int[5];

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        material = 100;
        for (int i = 0; i < 5; i++)
            listOfCosts[i] = maxCosts[i];
        upgradeUI.SetActive(false);
        UpdateTexts();
    }

    void UpdateTexts()
    {
        materialText.text = $"{material} Material";
        for (int i = 0; i<5; i++)
        {
            sliderForCosts[i].value = ((maxCosts[i] - (float)listOfCosts[i]) / maxCosts[i]);
            textForCosts[i].text = $"{listOfCosts[i]} Material";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            upgradeUI.SetActive(!upgradeUI.activeSelf);
            UIManager.instance.GetComponent<LockMouse>().LockCursor(!upgradeUI.activeSelf);
        }
    }

    public void AddMaterial(int x)
    {
        material += x;
        UpdateTexts();
    }

    public void LoseMaterial(int x)
    {
        material -= x;
        UpdateTexts();
    }

    public void SpendForUpgrade(int position)
    {
        if (material > 0 && listOfCosts[position] > 0)
        {
            listOfCosts[position]--;
            UpdateTexts();
            LoseMaterial(1);
        }
        else
            Debug.Log("can't upgrade");
    }
}
