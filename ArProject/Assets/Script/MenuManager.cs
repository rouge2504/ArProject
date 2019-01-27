using NatShareU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public GameObject[] menus;

	private float timeTitle;
    private float timingTitle;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private YoutubePlayer youtubePlayer;

    [SerializeField]
    private GameObject textureVideo;

    public static bool activeModel;

    private float timeToAnimation;
    private float timingToAnimation;

    private Vector3 initPosition;

    public static MenuManager instance;

    [HideInInspector]
    public string path;
	// Use this for initialization
	void Awake () {
        path = null;
        instance = this;
        timeToAnimation = 13;
        timingToAnimation = 0;
        initPosition = _animator.transform.position;
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

        CheckAnimation();
	}

    void CheckAnimation()
    {

        if (activeModel)
        {
            _animator.SetBool("activeAnimation", true);

            timingToAnimation += Time.deltaTime;
            if (timingToAnimation > timeToAnimation)
            {
                
                _animator.transform.Translate(0, -10f * Time.deltaTime, 0);

                if (_animator.transform.localPosition.y < -0.55f)
                {
                    _animator.gameObject.transform.position = initPosition;
                    timingToAnimation = 0;
                    _animator.Play("Die", -1, 0f);
                }
            }
        }
        else
        {
            _animator.SetBool("activeAnimation", false);
            timingToAnimation = 0;
            _animator.gameObject.transform.position = initPosition;
            _animator.Play("Die", -1, 0f);
        }
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

    public void ShareButton()
    {
        
        string videoPath = RecordVideo.instance.vuforiaCamera.vcObj.videoFilePath;
        print("Path: " + videoPath);
        NatShare.Share(videoPath);
    }
}
