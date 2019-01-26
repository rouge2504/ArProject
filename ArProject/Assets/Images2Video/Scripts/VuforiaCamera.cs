using UnityEngine;
#if ENABLE_Vuforia
using Vuforia;
#endif
using System;

namespace tw.com.championtek
{
    public class VuforiaCamera : MonoBehaviour
    {
#if (UNITY_IOS || UNITY_ANDROID)
        public VirtualCamera vcObj = null;
        private GameObject attachToARCamera = null;
#endif
        private Camera renderCamera = null;
        // Use this for initialization
        void Start()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            //Get game objects
            this.attachToARCamera = GameObject.Find("AttachToARCamera");
            vcObj = (VirtualCamera)this.attachToARCamera.GetComponent("VirtualCamera");

            renderCamera = vcObj.getRenderCamera();

#if ENABLE_Vuforia
            var vuforia = VuforiaARController.Instance;
            vuforia.RegisterBackgroundTextureChangedCallback(OnVuforiaBgTextureChanged);
            vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            vuforia.RegisterOnPauseCallback(OnPaused);
#endif
#endif
        }

        public void Begin()
        {
            Debug.Log("Begin shot!!");
#if (UNITY_IOS || UNITY_ANDROID)
            if (vcObj == null)
                return;

            vcObj.BeginShot();
#endif
        }

        public void End()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (vcObj == null)
                return;

            vcObj.EndShot();
#endif
        }

#if ENABLE_Vuforia
#if (UNITY_IOS || UNITY_ANDROID)
        private void OnVuforiaBgTextureChanged()
        {
            GameObject arCamera = GameObject.Find("ARCamera");

            if (this.attachToARCamera && arCamera)
            {
                this.attachToARCamera.transform.position = arCamera.transform.position;
                Camera cameraOfAR = arCamera.GetComponent<Camera>();
                renderCamera.orthographic = cameraOfAR.orthographic;
                renderCamera.nearClipPlane = cameraOfAR.nearClipPlane;
                renderCamera.farClipPlane = cameraOfAR.farClipPlane;
                renderCamera.depth = cameraOfAR.depth;

                int textureWidth = renderCamera.targetTexture.width;
                int textureHeight = renderCamera.targetTexture.height;
                float scaleRatio = 1.0f;

                if (textureWidth >= Screen.width || textureHeight >= Screen.height)
                {//Portrait
                    textureWidth = Screen.width - (Screen.width % 16);
                    textureHeight = Screen.height - (Screen.height % 2);
                }
                else
                {
                    textureWidth = textureWidth - (textureWidth % 16);
                    textureHeight = textureHeight - (textureHeight % 2);
                }

                renderCamera.fieldOfView = cameraOfAR.fieldOfView;
                vcObj.updateTextureSize(textureWidth, textureHeight);
                Debug.Log(String.Format("Screen: {0}x{1}, Texture: {2}x{3}, ratio: {4}, AR FOV: {5}, VR FOV: {6}",
                    Screen.width, Screen.height, textureWidth, textureHeight, scaleRatio, cameraOfAR.fieldOfView, renderCamera.fieldOfView));
            }
        }

        private void OnVuforiaStarted()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }

        private void OnPaused(bool paused)
        {
            if (!paused) // resumed
            {
                // Set again autofocus mode when app is resumed
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            }
        }
#endif
#endif
    }
}
