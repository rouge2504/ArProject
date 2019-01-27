using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace tw.com.championtek
{
    public class MixMP4 : MonoBehaviour
    {
        public string inputVideoName;
        public string inputAudioName;
        public string outputVideoName;
        public bool shortestClip;
        public bool saveToCameraRoll;
        private VideoConverter videoConverter;
        private string fullBasePath = "";
        private string baseFolder = "MixMP4";
        private string audioPath = "";
        private string videoPath = "";
        public void MixVideoAndAudio()
        {
            if (inputVideoName.Length <= 0)
                return;

            if (inputVideoName.Length <= 0 || outputVideoName.Length <= 0)
                return;

            if (!inputVideoName.ToLower().EndsWith(".mp4", StringComparison.Ordinal))
                inputVideoName += ".mp4";
            if (!outputVideoName.ToLower().EndsWith(".mp4", StringComparison.Ordinal))
                outputVideoName += ".mp4";

            // CreateDirectoryIfNotExist();
            fullBasePath = Utilties.CreateDirectoryIfNotExist(baseFolder);
#if (UNITY_IOS || UNITY_ANDROID)
            StartCoroutine(Mix());
#endif
        }

        IEnumerator Mix()
        {
            videoConverter = (VideoConverter)gameObject.GetComponent("VideoConverter");

            videoConverter.DisplayProgress("Converting", "Processing");

            // Get audio path
            string audioRawPath = System.IO.Path.Combine(Application.streamingAssetsPath, inputAudioName);
#if UNITY_IOS
                audioRawPath = String.Format("file://{0}", audioRawPath);
#endif
            Debug.Log("Audio path " + audioRawPath);
            WWW www = new WWW(audioRawPath);
            yield return www;

            // Download the audio file from server
            // WWW www = new WWW("http://www.championtek.com.tw/success.mp3");
            // yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Debug.Log("read audio file successed! and length => " + www.bytes.Length);
                audioPath = System.IO.Path.Combine(fullBasePath, inputAudioName);
                File.WriteAllBytes(audioPath, www.bytes);
                Debug.Log("write audio file to " + audioPath);
            }
            else
            {
                Debug.Log(String.Format("Get audio file from streaming assets folder error {0}", www.error));
            }

            //Get video path
            string videoRawPath = System.IO.Path.Combine(Application.streamingAssetsPath, inputVideoName);
#if UNITY_IOS
                videoRawPath = String.Format("file://{0}", videoRawPath);
#endif
            Debug.Log("video path " + videoRawPath);
            www = new WWW(videoRawPath);
            yield return www;

            // Download the audio file from server
            // WWW www = new WWW("http://www.championtek.com.tw/success.mp3");
            // yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Debug.Log("read video file successed! and length => " + www.bytes.Length);
                videoPath = System.IO.Path.Combine(fullBasePath, inputVideoName);
                File.WriteAllBytes(videoPath, www.bytes);
                Debug.Log("write video file to " + videoPath);
            }
            else
            {
                Debug.Log(String.Format("Get video file from streaming assets folder error {0}", www.error));
            }

            String attachedObjName = "default";
            if (gameObject != null)
                attachedObjName = gameObject.name;

            videoConverter.Mixing(attachedObjName, "End", videoPath, audioPath, outputVideoName, shortestClip, saveToCameraRoll);
        }

        /// Callback function when convertering finished
        /// </summary>
        /// <param name="videoPath">The generated video file path</param>
        public void End(string videoPath)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            Debug.Log("Callback End : " + videoPath);

            videoConverter.DismissProgress();
            videoConverter.DisplayMessage("The generated video path on MixMP4: " + videoPath);
#endif
        }
        // private void CreateDirectoryIfNotExist()
        // {
        //     fullBasePath = Path.Combine(Application.persistentDataPath, baseFolder);

        //     Debug.Log("MP4Encoder folder path - " + fullBasePath);

        //     DirectoryInfo directoryInfo = new DirectoryInfo(fullBasePath);

        //     if (!Directory.Exists(fullBasePath))
        //     {
        //         Directory.CreateDirectory(fullBasePath);
        //     }
        //     else
        //     {//clean all files
        //         FileInfo[] files = directoryInfo.GetFiles("*.*");
        //         foreach (FileInfo file in files)
        //         {
        //             file.Delete();
        //         }
        //     }
        // }
    }
}