using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTail : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPositions;
    private Vector3[] segmentVelocity;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    // public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    public Transform tailEnd;
    public Transform[] bodySegments;

    private void Start()
    {
        length = bodySegments.Length + 2;
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
            Vector3 targetPos = segmentPositions[i - 1] + (segmentPositions[i] - segmentPositions[i - 1]).normalized * targetDist;
            segmentPositions[i] = Vector3.SmoothDamp(segmentPositions[i], targetPos, ref segmentVelocity[i], smoothSpeed);
            if (i - 1 < bodySegments.Length)
            {
                bodySegments[i - 1].transform.position = segmentPositions[i];
            }
        }
        lineRenderer.SetPositions(segmentPositions);

        // tailEnd.position = segmentPositions[segmentPositions.Length - 1];
    }
}
