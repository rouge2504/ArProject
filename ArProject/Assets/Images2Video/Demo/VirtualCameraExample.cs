using UnityEngine;
#if(UNITY_IOS || UNITY_ANDROID)
using tw.com.championtek;
#endif

public class VirtualCameraExample : MonoBehaviour
{
#if (UNITY_IOS || UNITY_ANDROID)
    private VirtualCamera vcObj = null;
#endif
    private Spawn_Cube sc = null;

    void Start()
    {
#if (UNITY_IOS || UNITY_ANDROID)
        //Get game objects
        GameObject virtualCamera = GameObject.Find("VirtualCamera");
        vcObj = (VirtualCamera)virtualCamera.GetComponent("VirtualCamera");
#endif
        GameObject cubeSpawner = GameObject.Find("CubeSpawner");
        sc = (Spawn_Cube)cubeSpawner.GetComponent("Spawn_Cube");

        if (sc != null)
            sc.keepGoing = false;
    }

    public void Begin()
    {
        Debug.Log("Begin shot!!");
#if (UNITY_IOS || UNITY_ANDROID)
        if (vcObj == null)
            return;
#endif
        if (sc != null)
            sc.keepGoing = true;
#if (UNITY_IOS || UNITY_ANDROID)
        vcObj.BeginShot();
#endif
    }

    public void End()
    {
        Debug.Log("End shot!!");
#if (UNITY_IOS || UNITY_ANDROID)
        if (vcObj == null)
            return;
#endif
        if (sc != null)
            sc.keepGoing = false;
#if (UNITY_IOS || UNITY_ANDROID)
        vcObj.EndShot();
#endif
    }

    public void Interrupt()
    {
        Debug.Log("Interrupt shot!");
#if(UNITY_IOS || UNITY_ANDROID)
        if (vcObj == null)
            return;
#endif
        if (sc != null)
            sc.keepGoing = false;
#if (UNITY_IOS || UNITY_ANDROID)
        vcObj.InterruptShot();
#endif
    }
}
