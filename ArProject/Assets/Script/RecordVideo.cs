using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if(UNITY_IOS || UNITY_ANDROID)
using tw.com.championtek;
#endif
public class RecordVideo : MonoBehaviour {

    private GameObject attachToARCamera = null;
    [HideInInspector]
    public VuforiaCamera vuforiaCamera = null;

    public static RecordVideo instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
        this.attachToARCamera = GameObject.Find("AttachToARCamera");
        this.vuforiaCamera = (VuforiaCamera)this.attachToARCamera.GetComponent("VuforiaCamera");
    }

    public void BeginShot()
    {
        this.vuforiaCamera.vcObj.duration = 10;
        this.vuforiaCamera.Begin();

    }

    //IEnumerator BeginRec()
    //{
    //    this.vuforiaCamera.vcObj.duration = 10;
    //    this.vuforiaCamera.Begin();
    //    yield return new WaitForSeconds(10);
    //    this.vuforiaCamera.End();
    //}

    public void EndShot()
    {
        this.vuforiaCamera.End();
    }
}
