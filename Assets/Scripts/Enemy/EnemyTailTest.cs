using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTailTest : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPositions;
    private Vector3[] segmentVelocity;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPositions = new Vector3[length];
        segmentVelocity = new Vector3[length];
    }

    private void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude, 0);

        segmentPositions[0] = targetDir.position;

        for (int i = 1; i < segmentPositions.Length; i++)
        {
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], segmentPositions[i - 1] - targetDir.forward * targetDist, ref segmentVelocity[i], smoothSpeed + i / trailSpeed);
        }
        lineRenderer.SetPositions(segmentPositions);
    }
}
