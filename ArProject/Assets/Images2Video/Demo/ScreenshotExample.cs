using UnityEngine;
using System.Collections;
using System.IO;
using System;
using tw.com.championtek;

public class ScreenshotExample : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera renderCamera;

    Vector3 vrotate;
    VideoConverter videoConverter;

    private int frameRate = 10;// Setup fps
    private bool saveToCameraRoll = true;
    private bool keepGoing = false;
    private string audioFileName = "success.mp3";
    private string attachedObjName = "default";
    private string folder = "Screenshot";
    private string path = "";
    private int imageWidth = 0;
    private int imageHeight = 0;


    private Texture2D imageOverview;
    //	private int _count = 0;

    private int indicator = 0;

    void Awake()
    {
#if (UNITY_IOS || UNITY_ANDROID)
        QualitySettings.vSyncCount = 0;  // VSync must be disabled, then targetFrameRate work
        Application.targetFrameRate = frameRate;
#endif
    }

    // Use this for initialization
    void Start()
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.AutoRotation;

        // Use screen size as image size
#if (UNITY_IOS || UNITY_ANDROID)
            imageWidth = Screen.width;
            imageHeight = Screen.height;

            // Adjust the dimension of the render texture
            RenderTexture rt = renderCamera.targetTexture;
            rt.width = imageWidth;
            rt.height = imageHeight;

            videoConverter = (VideoConverter)gameObject.GetComponent("VideoConverter");
#endif

        int i = UnityEngine.Random.Range(1, 4);
        switch (i)
        {
            case 1:
                RotateLeft();
                break;
            case 2:
                RotateRight();
                break;
            case 3:
                RotateDown();
                break;
            case 4:
                RotateUp();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!keepGoing)
            return;

        Debug.Log("start rotate cube!");

        RotateCube();
    }

    void LateUpdate()
    {
        if (!keepGoing)
            return;
#if (UNITY_IOS || UNITY_ANDROID)
        StartCoroutine(takeScreenshot());
#endif
    }

    IEnumerator takeScreenshot()
    {
        yield return new WaitForEndOfFrame();
#if (UNITY_IOS || UNITY_ANDROID)
        Camera camera = renderCamera;

        RenderTexture currentRenderTexture = RenderTexture.active;

        RenderTexture.active = camera.targetTexture;

        camera.Render();

        imageOverview.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);

        imageOverview.Apply();

        RenderTexture.active = currentRenderTexture;

        // Encode texture to PNG
        byte[] bytes = imageOverview.EncodeToPNG();

        Debug.Log(String.Format("screenshot index : {0} and byte length : {1} image dimension : {2}x{3}", indicator, bytes.Length, imageWidth, imageHeight));

        videoConverter.ConvertImageToVideo(bytes, indicator);
#endif
        indicator++;
    }

    // Here we use ienumerator to trigger www and wait until download complete.
    IEnumerator InitialAllProperties()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            try {
                path = Utilties.CreateDirectoryIfNotExist(folder);
            } catch (Exception ex) {
                Debug.Log(ex.ToString());
            }
            

            // You can put the audio files into StreamingAssets folder or download from backend server
            // 1. Put the audio file in the StreamingAssets folder
            string base_path;
#if UNITY_EDITOR
            base_path = "file:" + Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
			base_path = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
			base_path = "file://" + Application.dataPath + "/Raw";
#else
			//Desktop (Mac OS or Windows)
			base_path = "file:"+ Application.dataPath + "/StreamingAssets";
#endif

            Debug.Log("read audio file => " + System.IO.Path.Combine(base_path, audioFileName));

            WWW www = new WWW(System.IO.Path.Combine(base_path, audioFileName));
            yield return www;

            // 2. Download the audio file from server
            // WWW www = new WWW("http://www.cupcake.tw/success.mp3");
            // yield return www;

            //Audio Path
            string file_path = "";

            if (string.IsNullOrEmpty(www.error))
            {
                Debug.Log("read audio file successed! and length => " + www.bytes.Length);
                file_path = System.IO.Path.Combine(path, audioFileName);

                File.WriteAllBytes(file_path, www.bytes);
                Debug.Log("write audio file to " + file_path);
            }

            if (gameObject != null)
                attachedObjName = gameObject.name;

            videoConverter.InitVars(attachedObjName, "End", "main.mp4", file_path, imageWidth, imageHeight, frameRate, false, saveToCameraRoll);

            Time.captureFramerate = frameRate;

            imageOverview = new Texture2D(renderCamera.targetTexture.width, renderCamera.targetTexture.height, TextureFormat.RGB24, false);
        }
        keepGoing = true;
    }

    public void ScreenshotBegin()
    {
        StartCoroutine("InitialAllProperties");
    }

    public void ScreenshotEnd()
    {
        ResetParameters();
        StopRotate();

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            videoConverter.DisplayProgress("Converting", "Processing");

            Destroy(imageOverview);

            videoConverter.ConvertImagesFinished();

        }
    }

    private void ResetParameters()
    {
        keepGoing = false;
        indicator = 0;
    }

    /// <summary>
    /// Callback function when convertering finished
    /// </summary>
    public void End(string videoPath)
    {
#if (UNITY_IOS || UNITY_ANDROID)
        Debug.Log("Save to camera roll finished and temporary video path is " + videoPath);
        videoConverter.DismissProgress();
        videoConverter.DisplayMessage(videoPath);
#endif
    }

    void RotateCube()
    {
        transform.Rotate(vrotate * Time.deltaTime * 100, Space.World);
    }

    void StopRotate()
    {
        transform.Rotate(0.0f, 0.0f, 0.0f);
    }

    void RotateLeft()
    {
        vrotate = Vector3.left;
    }

    void RotateRight()
    {
        vrotate = Vector3.right;
    }

    void RotateUp()
    {
        vrotate = Vector3.up;
    }

    void RotateDown()
    {
        vrotate = Vector3.down;
    }
}
