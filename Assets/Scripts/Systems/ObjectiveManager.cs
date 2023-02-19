using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public Objective[] ObjectiveArray = new Objective[20];
    public GameObject ObjectivePrefab;
    public Transform ObjectiveTransform;

    private int ObjectiveCount = 0;
    private int ObjectiveSpaceOnScreen = 0;

    private void Start()
    {
        instance = this;
        /*AddObejctive("Press 'F' for fun!");
        AddComplexObjective("Spell Gun!", new string[] { "Press 'G' ", "Press 'U' ", "Press 'N' " });
        AddObejctive("Press 'P' for pun!");
        AddObejctive("Press 'Z' for zun!");*/
        AddObejctive("Read Paper");
    }

    public void AddObejctive(string ObjectiveText)
    {
        int objectiveIndex = ObjectiveCount;
        Objective tempObejctive = Instantiate(ObjectivePrefab, ObjectiveTransform).GetComponent<Objective>();
        tempObejctive.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, ObjectiveSpaceOnScreen * -25);
        tempObejctive.GetComponent<TMP_Text>().text = ObjectiveText;
        tempObejctive.MyIndex = objectiveIndex;
        tempObejctive.MyText = ObjectiveText;
        for (int i = 0; i < ObjectiveArray.Length; i++)
        {
            if (ObjectiveArray[i] == null)
            {
                ObjectiveArray[i] = tempObejctive;
                tempObejctive.myPosition = i;
                break;
            }
        }
        ObjectiveCount++;
        ObjectiveSpaceOnScreen++;
    }

    public void AddComplexObjective(string MainObjective, string[] SubObjective)
    {
        int objectiveIndex = ObjectiveCount;
        Objective tempObejctive = Instantiate(ObjectivePrefab, ObjectiveTransform).GetComponent<Objective>();
        tempObejctive.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, ObjectiveSpaceOnScreen * -25);
        tempObejctive.GetComponent<TMP_Text>().text = MainObjective;
        tempObejctive.MyIndex = objectiveIndex;
        tempObejctive.MyText = MainObjective;
        for (int i = 0; i < ObjectiveArray.Length; i++)
        {
            if (ObjectiveArray[i] == null)
            {
                ObjectiveArray[i] = tempObejctive;
                tempObejctive.myPosition = i;
                break;
            }
        }
        ObjectiveCount++;
        ObjectiveSpaceOnScreen++;
        for (int i = 0; i < SubObjective.Length; i++)
        {
            tempObejctive.MySubObjective.Add(AddSubObejctive(SubObjective[i], tempObejctive));
        }

    }

    public Objective AddSubObejctive(string ObjectiveText, Objective MainObjective)
    {
        int objectiveIndex = ObjectiveCount;
        Objective tempObejctive = Instantiate(ObjectivePrefab, ObjectiveTransform).GetComponent<Objective>();
        tempObejctive.GetComponent<RectTransform>().anchoredPosition += new Vector2(20, ObjectiveSpaceOnScreen * -25);
        tempObejctive.GetComponent<TMP_Text>().fontSize -= 4;
        tempObejctive.GetComponent<TMP_Text>().text = "-" + ObjectiveText;
        tempObejctive.MyMainObjective = MainObjective;
        tempObejctive.MyIndex = objectiveIndex;
        tempObejctive.MyText = ObjectiveText;
        for (int i = 0; i < ObjectiveArray.Length; i++)
        {
            if (ObjectiveArray[i] == null)
            {
                ObjectiveArray[i] = tempObejctive;
                tempObejctive.myPosition = i;
                break;
            }
        }
        ObjectiveCount++;
        ObjectiveSpaceOnScreen++;
        return tempObejctive;
    }

    public void CompleteObjetive(string ObjetiveName)
    {
        for (int i = 0; i < ObjectiveArray.Length; i++)
        {
            if (ObjectiveArray[i] != null)
            {
                if(ObjetiveName == ObjectiveArray[i].MyText)
                {
                    if (ObjectiveArray[i].MySubObjective.Count < 1)
                    {
                        ObjectiveArray[i].FinishMe();
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CompleteObjetive("Press 'F' for fun!");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CompleteObjetive("Press 'P' for pun!");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CompleteObjetive("Press 'G' ");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CompleteObjetive("Press 'U' ");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            CompleteObjetive("Press 'N' ");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            CompleteObjetive("Press 'Z' for zun!");
        }
    }

}
