using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFile
{
    public bool boatDocked;

    public float currWatt;
    public float maxWatt;

    public float[] playerPosition = new float[3];
    public float[] playerRotation = new float[3];

    public float[] boatPosition = new float[3];
    public float[] boatRotation = new float[3];

    public DataFile(SaveManager SM, float current, float max)
    {
        this.boatDocked = SM.boatDocked;
        this.currWatt = current;
        this.maxWatt = max;

        this.playerPosition[0] = SM.playerPos.x;
        this.playerPosition[1] = SM.playerPos.y;
        this.playerPosition[2] = SM.playerPos.z;

        this.playerRotation[0] = SM.playerRot.x;
        this.playerRotation[1] = SM.playerRot.y;
        this.playerRotation[2] = SM.playerRot.z;

        this.boatPosition[0] = SM.boatPos.x;
        this.boatPosition[1] = SM.boatPos.y;
        this.boatPosition[2] = SM.boatPos.z;

        this.boatRotation[0] = SM.boatRot.x;
        this.boatRotation[1] = SM.boatRot.y;
        this.boatRotation[2] = SM.boatRot.z;
    }
}
