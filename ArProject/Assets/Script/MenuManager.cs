using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	[SerializeField]
    private GameObject[] menus;

	private float timeTitle;
    private float timingTitle;

    [SerializeField]
    private YoutubePlayer youtubePlayer;

    [SerializeField]
    private GameObject textureVideo;
	// Use this for initialization
	void Awake () {
		timeTitle = 4;
        timingTitle = 0;
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        menus[0].SetActive(true);

        youtubePlayer.OnVideoFinished.AddListener(DisableVideo);
    }
	
	// Update is called once per frame
	void Update () {
		TimeTitle();
	}

    public void PlayVideo()
    {
        youtubePlayer.enabled = false;
        youtubePlayer.autoPlayOnEnable = true;
        youtubePlayer.enabled = true;
        youtubePlayer.videoPlayer.Play();
        youtubePlayer.audioPlayer.Play();
    }

    private void DisableVideo()
    {
        textureVideo.SetActive(false);
        youtubePlayer.enabled = false;
        youtubePlayer.autoPlayOnEnable = false;
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
}
