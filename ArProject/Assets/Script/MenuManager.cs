using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	[SerializeField]
    private GameObject[] menus;

	private float timeTitle;
    private float timingTitle;
	// Use this for initialization
	void Start () {
		timeTitle = 4;
        timingTitle = 0;
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
        menus[0].SetActive(true);
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
}
