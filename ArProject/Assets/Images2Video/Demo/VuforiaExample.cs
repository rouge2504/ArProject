using UnityEngine;

#if(UNITY_IOS || UNITY_ANDROID)
using tw.com.championtek;
#endif
public class VuforiaExample : MonoBehaviour
{
    private GameObject attachToARCamera = null;
    private VuforiaCamera vuforiaCamera = null;
    // Use this for initialization
    void Start()
    {
        this.attachToARCamera = GameObject.Find("AttachToARCamera");
        this.vuforiaCamera = (VuforiaCamera)this.attachToARCamera.GetComponent("VuforiaCamera");
    }

    public void BeginShot()
    {
        this.vuforiaCamera.Begin();
    }

    public void EndShot()
    {
        this.vuforiaCamera.End();
    }
}
