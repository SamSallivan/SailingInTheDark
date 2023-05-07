using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public List<Objective> ObjectiveList;
    public GameObject ObjectivePrefab;
    public GameObject ObjectivePrefabAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

    }

    public void AssignObejctive(string ObjectiveText)
    {
        Objective newObjective = Instantiate(ObjectivePrefab, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        newObjective.GetComponent<TMP_Text>().text = ObjectiveText;
        ObjectiveList.Add(newObjective);
        StartCoroutine(ObjectiveAnimation(newObjective, ObjectiveText));
    }

    IEnumerator ObjectiveAnimation( Objective tempObjective, string ObjectiveText)
    {
        tempObjective.gameObject.SetActive(false);
        Objective newObjective = Instantiate(ObjectivePrefabAnim, UIManager.instance.objectiveUIAnim.transform).GetComponent<Objective>();
        newObjective.GetComponent<TMP_Text>().text = ObjectiveText;
        //newObjective.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0);
        yield return null;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(newObjective.GetComponent<TMP_Text>().DOColor(new Color(1, 1, 1, 1), 3)).PrependInterval(1).Append(newObjective.transform.DOScale(1f, 1))
          .Append(newObjective.GetComponent<TMP_Text>().DOFade(0, 0.5f).OnComplete(()
          => DestoryAndEnable(newObjective.gameObject,tempObjective.gameObject)));
    }

    public void DestoryAndEnable(GameObject destroyedObject, GameObject enabledObject)
    {
        Destroy(destroyedObject);
        enabledObject.SetActive(true);
    }

    /*public void AssignObejctive(Objective objective)
    {
        //Objective newObjective = Instantiate(ObjectivePrefab, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        //Objective newObjective = Instantiate(objective.gameObject, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        GameObject newObjective = Instantiate(objective.gameObject, UIManager.instance.objectiveUI.transform);//.GetComponent<Objective>();
        ObjectiveList.Add(newObjective);
        StartCoroutine(newObjective.GetComponent<Objective>().OnAssigned());
    }*/

    public void AssignObejctive(GameObject objective)
    {

        Objective newObjective = Instantiate(objective, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        newObjective.prefabRef = objective;
        ObjectiveList.Add(newObjective);
        StartCoroutine(ObjectiveAnimation(newObjective, newObjective.GetComponent<TMP_Text>().text));
        StartCoroutine(newObjective.OnAssigned());
    }

    /*public void AddComplexObjective(string MainObjective, string[] SubObjective)
    {
        Objective tempObejctive = Instantiate(ObjectivePrefab, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        tempObejctive.GetComponent<TMP_Text>().text = MainObjective;
        for (int i = 0; i < ObjectiveList.Count; i++)
        {
            if (ObjectiveList[i] == null)
            {
                ObjectiveList[i] = tempObejctive;
                break;
            }
        }
        for (int i = 0; i < SubObjective.Length; i++)
        {
            //tempObejctive.subObjectives.Add(AddSubObejctive(SubObjective[i], tempObejctive));
        }

    }*/

    /*public Objective AddSubObejctive(string ObjectiveText, Objective MainObjective)
    {
        Objective tempObejctive = Instantiate(ObjectivePrefab, UIManager.instance.objectiveUI.transform).GetComponent<Objective>();
        tempObejctive.GetComponent<TMP_Text>().fontSize -= 4;
        tempObejctive.GetComponent<TMP_Text>().text = "-" + ObjectiveText;
        //tempObejctive.mainObjective = MainObjective;
        for (int i = 0; i < ObjectiveList.Count; i++)
        {
            if (ObjectiveList[i] == null)
            {
                ObjectiveList[i] = tempObejctive;
                break;
            }
        }
        return tempObejctive;
    }*/

    public void CompleteObjetive(string ObjetiveName)
    {
        foreach (Objective objective in ObjectiveList)
        {
            if (ObjetiveName == objective.gameObject.name || ObjetiveName == objective.gameObject.GetComponent<TMP_Text>().text)
            {
                /*if (objective.subObjectives.Count <= 0)
                {
                }*/
                objective.Finish();
                break;
            }
        }
    }

    private void Update()
    {
    }

}
