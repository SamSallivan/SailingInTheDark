using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyBox;

[System.Serializable]
public class Objective : MonoBehaviour
{
    public bool finished;
    //public Objective mainObjective; 
    //public List<Objective> subObjectives = new List<Objective>();
    //public ObjectiveData objectiveData;

    public List<GameObject> newObjectives;
    public List<string> newObjectivesString;
    public float objectiveDelay;
    public DialogueData assignedDialogue;
    public DialogueData finishedDialogue;
    public float dialogueDelay;

    [ReadOnly()]
    public GameObject prefabRef;

    private void OnEnable()
    {
        GetComponent<TMP_Text>().DOFade(1,0.5f);
    }
    public void Update()
    {
        if (CheckFinished() && !finished)
        {
            Finish();
        }
    }

    public virtual IEnumerator OnAssigned()
    {

        if (assignedDialogue != null)
        {
            DialogueManager.instance.OverrideDialogue(assignedDialogue);
        }

        yield return null;
    }

    public virtual bool CheckFinished()
    {
        //objectiveData.onCheckFinished.Invoke();
        return false;
    }

    public void Finish()
    {
        finished = true;
        StartCoroutine(OnFinished());
    }

    public virtual IEnumerator OnFinished()
    {
        ObjectiveManager.instance.ObjectiveList.Remove(this);
        this.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.75f).OnComplete(() => CompleteFinished());

        yield return new WaitForSeconds(dialogueDelay);

        if (finishedDialogue != null)
        {
            DialogueManager.instance.OverrideDialogue(finishedDialogue);
        }

        foreach (GameObject objective in newObjectives)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        foreach (string objective in newObjectivesString)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        /*StartCoroutine(FinishDialogue());
        StartCoroutine(AssignObjective());*/

        Destroy(gameObject);
        yield return null;
    }

    public virtual IEnumerator FinishDialogue()
    {
        yield return new WaitForSeconds(dialogueDelay);

        if (finishedDialogue != null)
        {
            DialogueManager.instance.OverrideDialogue(finishedDialogue);
        }

        yield return null;
    }

    public virtual IEnumerator AssignObjective()
    {
        yield return new WaitForSeconds(objectiveDelay);

        foreach (GameObject objective in newObjectives)
        {
            Debug.Log("Started Coroutine at timestamp : " + Time.time);
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        foreach (string objective in newObjectivesString)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        yield return null;
    }

    public void CompleteFinished()
    {
        this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f);
        GetComponent<TMP_Text>().DOFade(0, 0.25f);

        /*if (mainObjective == null)
        {
            //this is NOT a sub objective
            if (subObjectives.Count <= 0)
            {
                //this is NOT a parent objective
                this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f);
                GetComponent<TMP_Text>().DOFade(0, 0.25f);
                
            }
            else
            {
                //this is a parent objective
                ObjectiveManager.instance.ObjectiveList.Remove(this);
                this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.3f);
                GetComponent<TMP_Text>().DOFade(0, 0.25f);
                for (int i = 0; i < subObjectives.Count; i++)
                {
                    subObjectives[i].Fademe();
                }

            }
        }
        else
        {
            //this is a sub objective
            if (mainObjective.CheckAllFinished())
            {
                mainObjective.Finish();
            }
        }*/
    }

    public void Fademe()
    {
        this.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.25f).OnComplete(() => Destroy(gameObject));
        GetComponent<TMP_Text>().DOFade(0, 0.25f);
    }


    public bool CheckAllFinished()
    {
        /*for (int i = 0; i < subObjectives.Count; i++)
        {
            if (subObjectives[i].finished == false)
            {
                return false;
            }
        }*/
        return true;
    }

}
