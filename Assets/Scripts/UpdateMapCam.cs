using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMapCam : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Camera>().orthographicSize = 400;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(
        PlayerController.instance.transform.position.x,
        400, PlayerController.instance.transform.position.z);
    }
}
