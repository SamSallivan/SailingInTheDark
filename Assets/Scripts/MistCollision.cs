using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistCollision : MonoBehaviour
{
    Slider hpSlider;
    public static MistCollision instance;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        hpSlider = GameObject.Find("HP Slider").GetComponent<Slider>();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SaveManager.instance.enabled = true;
        hpSlider.transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SaveManager.instance.alive && other.CompareTag("Mist"))
        {
            hpSlider.value = 1;
            hpSlider.transform.parent.gameObject.SetActive(true);
            StartCoroutine(DecreaseSlider());
            Debug.Log($"you entered mist");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mist"))
        {
            Debug.Log($"you exited mist");
            StopCoroutine(DecreaseSlider());
            hpSlider.transform.parent.gameObject.SetActive(false);
        }
    }

    public IEnumerator DecreaseSlider()
    {
        while (hpSlider.transform.parent.gameObject.activeSelf && hpSlider.value > 0)
        {
            hpSlider.value -= 0.01f/8f;
            yield return new WaitForSeconds(0.01f);
        }

        if (hpSlider.value <= 0)
        {
            Debug.Log("out of health");
            SaveManager.instance.Die("You were poisoned by the black mist.");
        }
    }
}
