using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Objective : MonoBehaviour
{
    public int MyIndex;
    public string MyText;
    public bool IsFinished;
    public Objective MyMainObjective; 
    public List<Objective> MySubObjective = new List<Objective>();
    public int myPosition;
    public bool Checked;

    private void OnEnable()
    {
        GetComponent<TMP_Text>().DOFade(1,0.5f);
    }

    public bool CheckAllFinished()
    {
        for (int i = 0; i < MySubObjective.Count; i++)
        {
            if (MySubObjective[i].IsFinished == false)
            {
                return false;
            }
        }
        Checked = true;
        return true;
    }

    public void FinishMe()
    {
        IsFinished = true;
        this.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.75f).OnComplete(() => CompleteFinished());
    }

    public void CompleteFinished()
    {
        if (MyMainObjective == null)
        {
            if (MySubObjective.Count < 1)
            {
                //this is a single objective
                this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f).OnComplete(() => Destroy(gameObject));
                GetComponent<TMP_Text>().DOFade(0, 0.25f);
                for (int i = myPosition; i < ObjectiveManager.instance.ObjectiveArray.Length - 1; i++)
                {
                    if (ObjectiveManager.instance.ObjectiveArray[i + 1] != null)
                    {
                        ObjectiveManager.instance.ObjectiveArray[i] = ObjectiveManager.instance.ObjectiveArray[i + 1];
                        ObjectiveManager.instance.ObjectiveArray[i].myPosition--;
                        float temp = ObjectiveManager.instance.ObjectiveArray[i].GetComponent<RectTransform>().anchoredPosition.y;
                        ObjectiveManager.instance.ObjectiveArray[i].GetComponent<RectTransform>().DOAnchorPosY(temp + 25, 0.5f + (i - myPosition)*0.2f);
                    }
                    else
                    {
                        ObjectiveManager.instance.ObjectiveArray[i] = null;
                        break;
                    }
                }
            }
            else
            {
                //this is a parent objective
                this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => Destroy(gameObject));
                GetComponent<TMP_Text>().DOFade(0, 0.25f);
                for (int i = 0; i < MySubObjective.Count; i++)
                {
                    MySubObjective[i].Fademe();
                }
                for (int i = 0; i < MySubObjective.Count + 1; i++)
                {
                    ObjectiveManager.instance.ObjectiveArray[myPosition + i] = null;
                }

                for (int i = myPosition + MySubObjective.Count + 1; i < ObjectiveManager.instance.ObjectiveArray.Length - 1; i++)
                {
                    if (ObjectiveManager.instance.ObjectiveArray[i] != null)
                    {
                        ObjectiveManager.instance.ObjectiveArray[i - MySubObjective.Count - 1] = ObjectiveManager.instance.ObjectiveArray[i];
                        ObjectiveManager.instance.ObjectiveArray[i - MySubObjective.Count - 1].myPosition -= MySubObjective.Count + 1;
                        ObjectiveManager.instance.ObjectiveArray[i] = null;
                        float temp = ObjectiveManager.instance.ObjectiveArray[i - MySubObjective.Count - 1].GetComponent<RectTransform>().anchoredPosition.y;
                        ObjectiveManager.instance.ObjectiveArray[i - MySubObjective.Count - 1].GetComponent<RectTransform>().
                            DOAnchorPosY(temp + 25 * (MySubObjective.Count + 1), 0.5f + (i - myPosition) * 0.2f);
                    }
                }


            }
        }
        else
        {
            //this is a sub objective
            if (MyMainObjective.Checked == false)
            {
                if (MyMainObjective.CheckAllFinished())
                {
                    MyMainObjective.FinishMe();
                }
            }
        }
    }

    public void Fademe()
    {
        this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f).OnComplete(() => Destroy(gameObject));
        GetComponent<TMP_Text>().DOFade(0, 0.25f);
    }

}
