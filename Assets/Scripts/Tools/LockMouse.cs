// by @torahhorse

using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour
{	
	void Start()
	{
		LockCursor(true);
	}

    void Update()
    {
    	// lock when mouse is clicked
        if( Input.GetMouseButtonDown(0) 
        && !UIManager.instance.inventoryUI.activeInHierarchy 
        && !UIManager.instance.examineUI.activeInHierarchy 
        && !UIManager.instance.gameOverUI.activeInHierarchy
        && !UIManager.instance.pauseUI.activeInHierarchy
        && !UIManager.instance.upgradeUI.activeSelf)
    	{
    		LockCursor(true);
    	}

    	// unlock when escape is hit
        if  ( Input.GetKeyDown(KeyCode.Escape))
        {
        	//LockCursor(Time.timeScale == 0.0f);
        }
    }
    
    public void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}