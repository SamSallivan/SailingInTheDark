using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text costText;
    public Slider progress;

    public int currentLevel;
    public int maxLevel;
}
