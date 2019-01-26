using UnityEngine;
using System.Collections;
using System;
using System.IO;

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace tw.com.championtek
{
    public class MP4Encoder : MonoBehaviour
    {
        public int frameRate;
        /// <summary>
        /// The subfolder name under StreamingAssets
        /// </summary>
        public string imageFolderName;
        public string imageExtensionName;
        public string inputAudioName;
        public string outputVideoName;
        public bool saveToCameraRoll;
        private bool shortestClip = false;
        private int imageWidth;
        private int imageHeight;

        private string baseFolder = "MP4Encoder";
        private string fullBasePath = "";
        private string audioPath = "";
        private string imageBasePath = "";

        private VideoConverter videoConverter;
        private DirectoryInfo directoryInfo;
        private string[] fileList = null;

        /// <summary>
        /// Encodes the video main entry
        /// </summary>
        public void EncodeVideo()
        {
            /*
             * Check the framerate input
             */
            if (frameRate == 0)
                return;

            /*
             * Check the extension name input
             */
            if (imageExtensionName.Length == 0)
                return;

            /*
             * Check the image folder name input
             */
            if (imageFolderName.Length == 0)
                return;

            /*
             * Currently we only support to generate mp4 format file
             */
            if (!outputVideoName.ToLower().EndsWith(".mp4", StringComparison.Ordinal))
                outputVideoName += ".mp4";

			/*
             * While all images are encoded, we should stop converting. So the shortestClip is not necessary.
             */
			shortestClip = false;

            fullBasePath = Utilties.CreateDirectoryIfNotExist(baseFolder);

#if (UNITY_IOS || UNITY_ANDROID)
            StartCoroutine(InitialAllProperties());
#endif
        }

        /// <summary>
        /// Initials all required properties before converting the video.
        /// </summary>
        /// <returns>The all properties.</returns>
        IEnumerator InitialAllProperties()
        {
            videoConverter = (VideoConverter)gameObject.GetComponent("VideoConverter");

            videoConverter.DisplayProgress("Converting", "Processing");

            // Check whether we need to process the audio file
            if (inputAudioName.Length > 0)
            {
                string audioRawPath = System.IO.Path.Combine(Application.streamingAssetsPath, inputAudioName);
#if UNITY_IOS
                audioRawPath = String.Format("file://{0}", audioRawPath);
                Debug.Log(String.Format("Temporary Cache Path : {0}", Application.temporaryCachePath));
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
            }
            else
            {
                //If only images need to be merged, then this parameter is useless
                shortestClip = false;
            }

            imageBasePath = System.IO.Path.Combine(Application.streamingAssetsPath, imageFolderName);

            // Read images data and get the dimension
#if UNITY_ANDROID
            fileList = videoConverter.GetAssetsContentList("images", imageExtensionName);
            imageBasePath = "jar:file://" + Application.dataPath + "!/assets/images";
#elif UNITY_IOS
            List<string> tempFileCollection = new List<string>();

            DirectoryInfo assetsDirectoryInfo = new DirectoryInfo(System.IO.Path.Combine(Application.streamingAssetsPath, imageFolderName));
            FileInfo[] files = assetsDirectoryInfo.GetFiles("*." + imageExtensionName);
            foreach (FileInfo file in files)
            {
                tempFileCollection.Add(file.Name);
            }

            // Convert to string[]
            fileList = tempFileCollection.ToArray();

            imageBasePath = String.Format("file://{0}", imageBasePath);
#endif
            foreach (String file in fileList)
            {
                if (imageWidth == 0 && imageHeight == 0)
                {
                    string imagePath = System.IO.Path.Combine(imageBasePath, file);
                    Debug.Log("imagePath " + imagePath);

                    WWW www = new WWW(imagePath);

                    yield return www;

                    if (string.IsNullOrEmpty(www.error))
                    {
                        Texture2D tempTexture = new Texture2D(1, 1);
                        tempTexture.LoadImage(www.bytes);
                        imageWidth = tempTexture.width;
                        imageHeight = tempTexture.height;

                        Debug.Log(String.Format("Image width {0} height {1}", imageWidth, imageHeight));

                        break;
                    }
                    else
                    {
                        Debug.Log(String.Format("Setup dimension error {0}", www.error));
                    }
                }
            }

            String attachedObjName = "default";
            if (gameObject != null)
                attachedObjName = gameObject.name;
#if (UNITY_IOS || UNITY_ANDROID)
            videoConverter.InitVars(attachedObjName, "End", outputVideoName, audioPath, imageWidth, imageHeight, frameRate, shortestClip, saveToCameraRoll);

            StartCoroutine(ReadImagesFromAssetsAndEncode(fileList));
#endif
        }

        public void Encoding(byte[] bytes, int index)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            videoConverter.ConvertImageToVideo(bytes, index);
#endif
        }

        public void EndEncoding()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            videoConverter.ConvertImagesFinished();
#endif
        }

        /// Callback function when convertering finished
        /// </summary>
        /// <param name="videoPath">The generated video file path</param>
        public void End(string videoPath)
        {
            Debug.Log("Callback End : " + videoPath);
#if (UNITY_IOS || UNITY_ANDROID)
            videoConverter.DismissProgress();
            videoConverter.DisplayMessage("The generated video path : " + videoPath);
#endif
        }

        IEnumerator ReadImagesFromAssetsAndEncode(string[] fileList)
        {
            int i = 1;
            foreach (String file in fileList)
            {
                string imagePath = System.IO.Path.Combine(imageBasePath, file);
                WWW www = new WWW(imagePath);
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    //				Debug.Log(String.Format("Encode {0} with index {1}", imagePath, i);
                    Encoding(www.bytes, i);
                    i++;
                }
            }
            EndEncoding();
        }

        // private void CreateDirectoryIfNotExist()
        // {
        //     fullBasePath = Path.Combine(Application.persistentDataPath, baseFolder);

        //     Debug.Log("MP4Encoder folder path - " + fullBasePath);

        //     directoryInfo = new DirectoryInfo(fullBasePath);

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
