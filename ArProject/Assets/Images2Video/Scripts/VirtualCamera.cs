using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;
using System.Collections;
using System;
using System.IO;
using tw.com.championtek.Audio;

namespace tw.com.championtek
{
    public class VirtualCamera : MonoBehaviour
    {
        public int frameRate = 30;
        public VideoDimension videoDimension = VideoDimension.RenderTextureSize;
        public string outputVideoName;
        public bool saveToCameraRoll = true;
        public RecordType recordType = RecordType.None;
        public GameObject mainAudio;
        public int duration = 0;//seconds

        private Camera renderCamera;
        private VideoConverter videoConverter;
        private int indicator;
        private bool recording;
        private int textureWidth, textureHeight;
        private RenderTexture rt = null;
        private string audioFileName = "recorded_audio.wav";
        private string audioExtension = ".wav";
        private int audioSampleRate;
        private AudioSource source;
        private float previousVolume;
        private RecordInGames recordInGames;
        private EventForwarder forwarder;
        private bool enableMicPermission;
        float deltaTime = 0.0f;

        private float volume = 1.0f;
        private int end_index = 0;
        private string videoFilePath;
        public void BeginShot()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (recording)
                return;

            indicator = 0;
            // Preserve the previous audio clip before recording from microphone
            if (source != null)
            {
                previousVolume = source.volume;
                source.volume = previousVolume * 0.3f;
            }

            // First check the output video name
            if (outputVideoName.Length == 0)
                outputVideoName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";

            // Second check the extension name
            if (!outputVideoName.ToLower().EndsWith(".mp4", StringComparison.Ordinal))
                outputVideoName += ".mp4";

            if (source != null)
                videoConverter.RegisterSystemVolumeNotification(this.name, "onSystemVolumeChanged");

            StartCoroutine(InitialAllProperties());
#endif
        }

        public void EndShot()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            StopAll((int)StopType.Stop);
#endif
        }
        public void InterruptShot()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            StopAll((int)StopType.Interrupt);
#endif
        }

        private void StopAll(int type)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (!recording)
                return;

            recording = false;
            FinalizeRecording();

            videoConverter.DisplayProgress("Converting", "Processing");
            if (type == (int)StopType.Stop)
                videoConverter.ConvertingTextureFinished();
            else
                videoConverter.InterruptEncoding();

            // Reset the previous audio clip after recording from microphone
            if (source != null && recordType == RecordType.FromMic)
            {
                source.volume = previousVolume;
            }

            if (source != null)
                videoConverter.UnregisterSystemVolumeNotification();
#endif
        }

        private void FinalizeRecording()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (recordType == RecordType.InGames)
            {
                if (recordInGames != null)
                    recordInGames.StopRecording();
                recordInGames = null;
            }
            else if (recordType == RecordType.FromMic)
            {
                videoConverter.StopRecordFromMicrophone();
            }
            else
            {

            }
#endif
        }

        void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            QualitySettings.vSyncCount = -1;  // VSync must be disabled, then targetFrameRate work
            Application.targetFrameRate = frameRate;
            Debug.Log(String.Format("VideoConverter TargetFrameRate : {0}", Application.targetFrameRate));

            //Initialize parameters
            videoConverter = (VideoConverter)gameObject.GetComponent("VideoConverter");
            renderCamera = gameObject.GetComponent<Camera>();

            rt = renderCamera.targetTexture;

            textureWidth = Screen.width;
            textureHeight = Screen.height;

            Assert.IsNotNull(rt, "Please attach the render texture to target camera!");

            if (rt != null && videoDimension == VideoDimension.RenderTextureSize)
            {
                textureWidth = rt.width;
                textureHeight = rt.height;
                Debug.Log(String.Format("[INFO] Video dimension is render texture size"));
            }
            else
            {
                Debug.Log(String.Format("[INFO] Video dimension is main camera size"));
            }

            // Simple way to correct the video dimension, which the width can only be divided by 16
            textureWidth = textureWidth - (textureWidth % 16);
            textureHeight = textureHeight - (textureHeight % 2);

            // Reset the dimension of rt 
            rt.width = textureWidth;
            rt.height = textureHeight;

            // if (videoDimension == VideoDimension.MainCameraSize)
            // {
            //     //Reset the render texture dimension
            //     // RenderTexture newRenderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            //     // newRenderTexture.depth = 24;
            //     // newRenderTexture.antiAliasing = 0;
            //     // newRenderTexture.wrapMode = TextureWrapMode.Clamp;
            //     // newRenderTexture.filterMode = FilterMode.Point;
            //     // newRenderTexture.Create();
            //     // renderCamera.targetTexture = newRenderTexture;
            //     // RenderTexture.active = newRenderTexture;
            //     rt.width = textureWidth;
            //     rt.height = textureHeight;
            // }
            // else
            // {
            //     rt.width = textureWidth;
            //     rt.height = textureHeight;
            // }
            Debug.Log(String.Format("VideoDimension : {0} VideoConverter : {1}  Render Camera : {2} preview width : {3} and height {4}",
        videoDimension, videoConverter, renderCamera, textureWidth, textureHeight));
#endif
        }

        // Use this for initialization
        void Start()
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (mainAudio != null)
            {
                source = mainAudio.GetComponent<AudioSource>();
                Debug.Log(String.Format("From {0} Get Audio Source Object {1}", mainAudio, source));
            }

            if (recordType == RecordType.InGames)
            {
                forwarder = gameObject.AddComponent<EventForwarder>();
                forwarder.OnAudioFilterReadEvent += HandleOnAudioFilterReadEvent;
            }
            else if (recordType == RecordType.FromMic)
            {
                StartCoroutine(CheckAuthorizeMicrophone());
                //Call the native API
            }

            if (frameRate == 0)
                return;
            // rt = renderCamera.targetTexture;
            // if (outputVideoName.Length == 0)
            // 	outputVideoName = "images2video.mp4";//use default name
            audioSampleRate = AudioSettings.outputSampleRate;
#endif
        }

        /// <summary>
        /// Calculate the fps
        /// </summary>
        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            if ((duration > 0) && (indicator == end_index))
                EndShot();

        }

        /// <summary>
        /// Get the audio path.
        /// </summary>
        /// <returns>The audio path string</returns>
        public string GetAudioPath()
        {
            string path = "";
#if (UNITY_IOS || UNITY_ANDROID)
            if (recordType == RecordType.FromMic || recordType == RecordType.InGames)
            {
                if (!audioFileName.ToLower().EndsWith(audioExtension, StringComparison.Ordinal))
                    audioFileName += audioExtension;
                path = Path.Combine(Application.persistentDataPath, audioFileName);
            }

            /// If you select record type from file which generated by the other plugin, please return this saved file path
            if (recordType == RecordType.FromPlugin)
                path = "";
#endif
            return path;
        }

        IEnumerator CheckAuthorizeMicrophone()
        {
            // Currently this feature only support WebPlayer!!
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
            Debug.Log(String.Format("Request authorization for microphone {0}", Application.HasUserAuthorization(UserAuthorization.Microphone)));
            if (Application.HasUserAuthorization(UserAuthorization.Microphone))
                enableMicPermission = true;
            else
                enableMicPermission = false;
        }

        /// <summary>
        /// Initials all properties.
        /// 
        /// <remark> 
        /// If you select record type with from file which is generated by the other plugin, 
        /// then you need to pass the audio file saved path while initializing.
        /// </remark>
        /// 
        /// </summary>
        /// <returns>The all properties.</returns>
        IEnumerator InitialAllProperties()
        {
            Debug.Log(String.Format(
                "RenderCamera : {0} TargetTexture : {1} ColorBuffer : {2} ColorBuffer Ptr : {3} DepthBuffer : {4} DepthBuffer Ptr : {5}",
                renderCamera, renderCamera.targetTexture,
                renderCamera.targetTexture.colorBuffer, renderCamera.targetTexture.colorBuffer.GetNativeRenderBufferPtr(),
                renderCamera.targetTexture.depthBuffer, renderCamera.targetTexture.depthBuffer.GetNativeRenderBufferPtr()
            ));
#if UNITY_ANDROID
            videoConverter.InitVarsWithTextureId(
                this.name,
                "End",
                outputVideoName,
                GetAudioPath(),
                recordType,
                textureWidth,
                textureHeight,
                frameRate,
                true,
                renderCamera.targetTexture.GetNativeTexturePtr(),
                renderCamera.targetTexture.colorBuffer.GetNativeRenderBufferPtr(),
                renderCamera.targetTexture.depthBuffer.GetNativeRenderBufferPtr(),
            saveToCameraRoll);
#elif UNITY_IOS
            videoConverter.InitVarsWithBuffers(
                this.name,
                "End",
                outputVideoName,
                GetAudioPath(),
                textureWidth,
                textureHeight,
                frameRate,
                true,
                renderCamera.targetTexture.GetNativeTexturePtr(),
                renderCamera.targetTexture.colorBuffer.GetNativeRenderBufferPtr(),
                renderCamera.targetTexture.depthBuffer.GetNativeRenderBufferPtr(),
                saveToCameraRoll
            );
#endif

#if (UNITY_IOS || UNITY_ANDROID)
            if (recordType == RecordType.InGames)
            {
                recordInGames = new RecordInGames(GetAudioPath(), audioSampleRate);
                recordInGames.StartRecording();
            }
            else if (recordType == RecordType.FromMic)
            {
#if UNITY_IOS
                videoConverter.PrepareRecordFromMic();
                videoConverter.StartRecordFromMicrophone(audioSampleRate);
#elif UNITY_ANDROID
                videoConverter.StartRecordFromMicrophone(audioSampleRate);
#endif
            }
            else if (recordType == RecordType.FromPlugin)//Merge the audio file from 3rd party
            {
                //Nothing
            }
            else
            {

            }
#endif
            yield return null;

            recording = true;

            // //Delay 6 seconds then stop shotting
            // Invoke("EndShot", 6);
        }

        // Set it to true so we can watch the flipped Objects
        void OnPreRender()
        {
            // GL.invertCulling = true;
        }
        void OnPostRender()
        {
            // Set it to false again because we dont want to affect all other cammeras.
            // GL.invertCulling = false;
#if (UNITY_IOS || UNITY_ANDROID)
            // It will be called after a camera has finished rendering the scene.
            if (recording)
            {
                float fps = 1.0f / deltaTime;
                videoConverter.ConvertFrameBufferToVideo(indicator, textureWidth, textureHeight, Convert.ToInt32(fps));
                // videoConverter.ConvertFramebufferToVideo(indicator, textureWidth, textureHeight);
                indicator++;
            }
#endif
        }

        /// <summary>
        /// OnAudioFilterReadEvent handler and only work while recordType is RecordType.InGame
        /// </summary>
        /// <param name="data"></param>
        /// <param name="numChannels"></param>
        private void HandleOnAudioFilterReadEvent(float[] data, int numChannels)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (recording)
            {
                Int16[] intData = new Int16[data.Length];//float[] to Int16[]
                Byte[] bytesData = new Byte[data.Length * 2];//Int16[] to Byte[]

                for (int i = 0; i < data.Length; i++)
                {
                    float newData = data[i];

                    //Gain control
                    if (Mathf.Max(newData) > 1.0f)
                        newData = 1.0f;
                    if (Mathf.Min(newData) < -1.0f)
                        newData = -1.0f;

                    intData[i] = (short)(newData * Consts.Rescale_Factor * volume);
                    Byte[] byteArray = new Byte[2];
                    byteArray = BitConverter.GetBytes(intData[i]);
                    byteArray.CopyTo(bytesData, i * 2);

                    byteArray = null;
                }

                intData = null;

                recordInGames.ConvertAndWrite(bytesData);
            }
#endif
        }

        /// <summary>
        /// Callback function when convertering finished
        /// </summary>
        /// <param name="videoPath">The generated video file path</param>
        public void End(string videoPath)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            Debug.Log("Callback End : " + videoPath);
            videoConverter.DismissProgress();
            videoConverter.DisplayMessage("The generated video path : " + videoPath);

            //MP4Player mPlayer = new MP4Player();
            //mPlayer.PlayVideo(videoPath);

            this.videoFilePath = videoPath;

            if (!saveToCameraRoll)
                videoConverter.SaveToCameraRoll(this.name, "OnSavedToCameraRollEnd", videoPath);
#endif
        }

        public void OnSavedToCameraRollEnd(string videoPath)
        {
            Debug.Log("Callback OnSavedToCameraRollEnd : " + videoPath);
        }

        /// <summary>
        /// Callback function to notify the system audio volume changed.
        /// </summary>
        /// <param name="volume">Volume.</param>
        public void onSystemVolumeChanged(string volume)
        {
            Debug.Log("Callback volume : " + volume);
            if (source != null)
                this.volume = float.Parse(volume);
        }

        public void OnFrameRateUpdate(string fps)
        {
            if (duration > 0)
            {
                int f = Int32.Parse(fps);
                end_index = (duration + 1) * f;
                Debug.Log(String.Format("FrameRateUpdate : {0} end at : {1}", f, end_index));
            }
        }

        /// <summary>
        /// Monitor converting progress. Currently this is for test and not ready to use
        /// </summary>
        /// <param name="progess">Progess.</param>
        public void OnConvertProgress(string progess)
        {
            Debug.Log(String.Format("Converting progress : {0}", float.Parse(progess)));
        }

        /// <summary>
        /// Ready for recording the audio, currently this feature is on testing.
        /// </summary>
        /// <param name="flag">Flag.</param>
        public void OnReadyForRecordingAudio(string gapSize)
        {
#if UNITY_IOS
            int g = Int32.Parse(gapSize);
            end_index = end_index + g;
#endif
        }

        public Camera getRenderCamera()
        {
            return this.renderCamera;
        }

        public void updateTextureSize(int width, int height)
        {
            this.textureWidth = width;
            this.textureHeight = height;
        }

        public void ShareVideoToWhatsApp()
        {
            ShareVideo(SNSType.WhatsApp);
        }
        public void ShareVideoToLine()
        {
            ShareVideo(SNSType.LINE);
        }
        public void ShareVideoToWeChat()
        {
            ShareVideo(SNSType.WeChat);
        }
        public void ShareVideoToSMS()
        {
            ShareVideo(SNSType.SMS);
        }

        private void ShareVideo(SNSType type)
        {
            videoConverter.ShareVideoToSNS(type, this.videoFilePath);
        }
    }
}