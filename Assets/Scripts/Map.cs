using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [Header("Player")]
    public Transform playerObject;
    public Transform lighthouseObject;

    [Header("Map")]
    public Transform playerIcon;
    public Transform lighthouseIcon;

    // Update is called once per frame
    void Update()
    {
        lighthouseIcon.transform.localPosition = new Vector3
        (lighthouseObject.transform.localPosition.x/500f, lighthouseObject.transform.localPosition.z/ 500f, -1);

        playerIcon.transform.localPosition = new Vector3
        (playerObject.transform.localPosition.x/500f, playerObject.transform.localPosition.z/ 500f, -1);

    }
}
