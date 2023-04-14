using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Enemy : MonoBehaviour
{
    public string targetTag = "Light";
    public EnemyMovement enemy;

    private void Start()
    {
        enemy = transform.parent.GetComponent<EnemyMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            //TODO: GET LIGHT INTENSITY.
            Light light = other.gameObject.GetComponent<Light>();
            if (light != null)
            {
                enemy.EnterLight(light.intensity);
            }
            else
            {
                enemy.EnterLight();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {
            enemy.ExitLight();
        }
    }
}
