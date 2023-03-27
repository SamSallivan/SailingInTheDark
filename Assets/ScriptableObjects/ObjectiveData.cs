using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ObjectiveData", menuName = "ScriptableObjects/ObjectiveData")]
public class ObjectiveData : ScriptableObject
{
    public Objective objective;

    public UnityEvent onCheckFinished;

}
