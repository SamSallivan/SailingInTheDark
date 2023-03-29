using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class T_Mist : Trigger
{
    [ReadOnly]
    public bool inMist = false;
    [ReadOnly]
    public float mistTimer;
    public float deathTime = 8;
    public float replenishCoeffcient = 2;

    public void Start(){
        mistTimer = 0;
    }

    public override IEnumerator TriggerEvent(){
        if(SaveManager.instance.alive){
            inMist = true;
            //UIManager.instance.mistSlider.value = 1;
            //Debug.Log($"you entered mist");
        }
        yield break;
    }


    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            inMist = false;
        }
    }

    public void Update(){
        if(inMist){
            UIManager.instance.mistUI.SetActive(true);
            mistTimer += Time.fixedDeltaTime;

            if(mistTimer > deathTime){
                SaveManager.instance.Die("Killed by mist");
            }
        }
        else{
            if(mistTimer > 0){
                mistTimer -= Time.fixedDeltaTime * replenishCoeffcient;
            }
            else{
                UIManager.instance.mistUI.SetActive(false);
            }
        }
        
            UIManager.instance.mistSlider.value = mistTimer/deathTime;
    }
}
