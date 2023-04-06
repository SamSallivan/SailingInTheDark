using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class InventoryAudio : MonoBehaviour
{
    [Foldout("AudioSources", true)]
    public AudioSource inventorySound;

    [Foldout("AudioClips", true)]
    public AudioClip sfx_inv_open;
    public AudioClip sfx_inv_close;
    public AudioClip sfx_item_collect;
    public AudioClip sfx_item_drop;
    public AudioClip sfx_item_equip;
    public AudioClip sfx_inv_hover_inspect; //for keyboard and click
    public AudioClip sfx_inv_hover_empty; //for mouse

    public void PlayInventoryOpen()
    {
        inventorySound.clip = sfx_inv_open;
        inventorySound.Play();
    }
    public void PlayInventoryClose()
    {
        inventorySound.clip = sfx_inv_close;
        inventorySound.Play();
    }
    public void PlayItemCollect()
    {
        inventorySound.clip = sfx_item_collect;
        inventorySound.Play();
    }

    public void PlayItemDrop()
    {
        inventorySound.clip = sfx_item_drop;
        inventorySound.Play();
    }

    public void PlayItemEquip()
    {
        inventorySound.clip = sfx_item_equip;
        inventorySound.Play();
    }

    public void PlayItemUnequip()
    {
        inventorySound.pitch = 0.3f;
        inventorySound.clip = sfx_item_equip;
        inventorySound.Play();
        inventorySound.pitch = 1f;
    }


    public void PlayHoverInspect()
    {
        inventorySound.clip = sfx_inv_hover_inspect;
        inventorySound.Play();
    }

    public void PlayHoverEmpty()
    {
        inventorySound.clip = sfx_inv_hover_empty;
        inventorySound.Play();
    }

}
