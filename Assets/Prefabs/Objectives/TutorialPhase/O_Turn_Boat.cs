using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Turn_Boat : Objective
{
    float maxTime = 5f;
    float timer = 0;
    public override bool CheckFinished()
    {
        if (Mathf.Abs(BoatController.instance.helm.wheelAngle) >= 85f)
        {
            timer += Time.deltaTime;
        }

        if (timer >= maxTime)
        {
            return true;
        }
        return false;
    }
}
