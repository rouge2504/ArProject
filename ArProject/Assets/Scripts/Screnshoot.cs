using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screnshoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*ScreenshotManager.OnScreenshotTaken += ScreenshotTaken;
        ScreenshotManager.OnScreenshotSaved += ScreenshotSaved; */
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnSaveImagePress()
    {
		ScreenshotManager.SaveScreenshot("MyScreenshot", "FIDH", "jpeg");
    }
}
