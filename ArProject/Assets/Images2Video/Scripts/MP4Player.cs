using UnityEngine;

namespace tw.com.championtek
{
    public class MP4Player : MonoBehaviour
    {
        private GameObject mainCamera;
        public MP4Player()
        {

        }

#if UNITY_5_6
        public void PlayVideo(string url)
        {
            mainCamera = GameObject.Find("Main Camera");
            var videoPlayer = mainCamera.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCameraAlpha = 1.0F;
            videoPlayer.url = url;
            videoPlayer.isLooping = true;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.Play();
        }

        void EndReached(UnityEngine.Video.VideoPlayer vp)
        {
            //Do anything while the video reaches the end.

            //Release game object
            Destroy(vp);
        }
#endif
    }
}
