using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screnshoot : MonoBehaviour
{
    private bool isProcessing = false;
    Texture2D imageTaken;
    string tempSharePath;
    // Use this for initialization
    void Start()
    {
        //ScreenshotManager.OnScreenshotTaken += ScreenshotTaken;
        /*ScreenshotManager.OnScreenshotSaved += ScreenshotSaved; */

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnSaveImagePress()
    {
        ScreenshotManager.SaveScreenshot("MyScreenshot", "FIDH", "jpeg");
        ScreenshotManager.OnScreenshotTaken += ScreenshotTaken;
        string ScreenshotName = "screenshot.png";
        tempSharePath = Application.persistentDataPath + "/" + ScreenshotName;
        if (File.Exists(tempSharePath)) File.Delete(tempSharePath);
        ScreenCapture.CaptureScreenshot(ScreenshotName);
    }

    private void ScreenshotTaken(Texture2D image)
    {
        Debug.Log("Obteniendo imagen");
        imageTaken = image;
    }

    public void ShareImageAndroid()
    {
#if UNITY_ANDROID
        //AndroidShare();
#endif
    }

    public void ShareButton()
    {
        Share("", tempSharePath, "");
    }

    private void Share(string shareText, string imagePath, string url, string subject = "")
    {
#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("setType", "image/png");

        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
        currentActivity.Call("startActivity", jChooser);
#elif UNITY_IOS
		CallSocialShareAdvanced(shareText, subject, url, imagePath);
#else
		Debug.Log("No sharing set up for this platform.");
#endif
    }

}
