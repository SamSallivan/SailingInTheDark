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
        if (GameOverManager.instance.gameOver)
        {
            LockCursor(false);
        }

    	else if( Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f && !UIManager.instance.inventoryUI.activeInHierarchy && !UIManager.instance.examineUI.activeInHierarchy)
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