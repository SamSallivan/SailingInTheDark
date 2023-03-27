using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Objective : Trigger
{
    public List<string> oldObjectives;
    public float newObjectiveDelay;
    public List<Objective> newObjectives;
    public List<string> newObjectivesString;

    public override IEnumerator TriggerEvent()
    {
        foreach (string objective in oldObjectives)
        {
            ObjectiveManager.instance.CompleteObjetive(objective);
        }

        yield return new WaitForSeconds(newObjectiveDelay);

        foreach (Objective objective in newObjectives)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        foreach (string objective in newObjectivesString)
        {
            ObjectiveManager.instance.AssignObejctive(objective);
        }

        yield return null;

    }
}
