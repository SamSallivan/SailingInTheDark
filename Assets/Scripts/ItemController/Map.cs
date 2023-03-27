using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    [Header("Player")]
    public Transform playerObject;
    public Transform lighthouseObject1;
    public Transform lighthouseObject2;

    [Header("Map")]
    public RectTransform playerIcon;
    public RectTransform lighthouseIcon1;
    public RectTransform lighthouseIcon2;

    public float scale;

    // Update is called once per frame
    void Update()
    {
        lighthouseIcon1.localPosition = new Vector3
        (lighthouseObject1.transform.position.x/ scale, lighthouseObject1.transform.position.z/ scale, 0);

        lighthouseIcon2.localPosition = new Vector3
            (lighthouseObject2.transform.position.x / scale, lighthouseObject2.transform.position.z / scale, 0);

        playerIcon.localPosition = new Vector3
        (playerObject.transform.position.x/ scale, playerObject.transform.position.z/ scale, 0);

    }
}
