using NatCorderU.Core;
using NatCorderU.Core.Clocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] menus;

    [SerializeField]
    private VuforiaMonoBehaviour vuforia;
    private float timeTitle;
    private float timingTitle;


    Gyroscope m_Gyro;

    [SerializeField]
    private GameObject baseGO;

    [SerializeField]
    private GameObject placaGO;

    //public RawImage raw;

    //IClock recordingClock;
    //WebCamTexture cameraTexture;
    // Use this for initialization
    void Start () {
        timeTitle = 4;
        timingTitle = 0;
		for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        menus[0].SetActive(true);
        vuforia.enabled = false;

        //cameraTexture = new WebCamTexture();
        //cameraTexture.Play();
        //raw.texture = cameraTexture;
	}
	
	// Update is called once per frame
	void Update () {
        TimeTitle();	

	}

    private void TimeTitle()
    {
        if (menus[0].activeSelf)
        {
            timingTitle += Time.deltaTime;
            if (timingTitle > timeTitle)
            {
                menus[0].SetActive(false);
                menus[1].SetActive(true);
            }
        }

       /* if (Input.GetKeyDown(KeyCode.A))
        {
            Recorder();
        }*/
    }

    public void GetPage(GameObject page)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        page.SetActive(true);
    }

    public void GetPageVuforia(GameObject page)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        page.SetActive(true);
        vuforia.enabled = true;
        if (m_Gyro.enabled)
        {
            placaGO.SetActive(true);
            baseGO.SetActive(false);
        }
        else
        {
            placaGO.SetActive(false);
            baseGO.SetActive(true);
        }
    }

    /* IEnumerator RecorderCourutine()
     {
         Debug.Log("Empezando a grabar");
         recordingClock = new RealtimeClock();

         NatCorder.StartRecording(Container.MP4, VideoFormat.Screen, AudioFormat.None, OnVideo);
         // Wait for 10 seconds
         StartCoroutine(RecordLoop());
         yield return new WaitForSeconds(10f);
         // Stop recording
         // The `OnVideo` callback will be invoked with the path to the recorded video
         NatCorder.StopRecording();
         Debug.Log("GRabado");
     }

     IEnumerator RecordLoop()
     {
         while (NatCorder.IsRecording)
         {
             // Acquire an encoder frame from NatCorder
             var frame = NatCorder.AcquireFrame();
             // Blit the current camera preview frame to the encoder frame
             Graphics.Blit(cameraTexture, frame);
             // Commit the frame to NatCorder for encoding
             NatCorder.CommitFrame(frame, recordingClock.CurrentTimestamp);
             // Wait for the end of the application frame
             yield return new WaitForEndOfFrame();
         }
     }

     public void Recorder()
     {
         Debug.Log("Entrando en la funcion Recorder");
         StartCoroutine(RecorderCourutine());
     }*/


    void OnVideo(string path)
    {
        print("path");
        path = Application.persistentDataPath + "/videoprueba";
        print("path: " + path);
    }
}
