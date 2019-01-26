using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace tw.com.championtek
{
    public class VideoConverter : MonoBehaviour
    {
#if UNITY_ANDROID
        private AndroidJavaClass _androidClass;
        private AndroidJavaObject _activity;
        private AndroidJavaObject _javaClass;
#endif
        void Awake()
        {
#if UNITY_IOS
            //Nothing
#elif UNITY_ANDROID
            _androidClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _activity = _androidClass.GetStatic<AndroidJavaObject>("currentActivity");
            _javaClass = new AndroidJavaObject("tw.com.championtek.videoconverter.VideoConverter", _activity);
#endif
        }

        /// <summary>
        /// Mix the mp4 video and mp3/Wave audio to mp4 video.
        /// </summary>
        /// <param name="gameObjName">The attached game object name.</param>
        /// <param name="callbackMethod">The callback method which will be called by UnitySendMessage.</param>
        /// <param name="inputVideoPath">The input mp4 video file path.</param>
        /// <param name="inputAudioPath">The input mp3/Wave audio file path.</param>
        /// <param name="outputVideoName">The output mp4 video name.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        public void Mixing(String gameObjName, String callbackMethod, String inputVideoPath, String inputAudioPath, String outputVideoName, bool shortestClip, bool saveToCameraRoll)
        {
#if UNITY_IOS
            MixVideoAndAudio(gameObjName, callbackMethod, inputVideoPath, inputAudioPath, outputVideoName, shortestClip, saveToCameraRoll);
#elif UNITY_ANDROID
            object[] _params = {
                        gameObjName,
                        callbackMethod,
                        inputVideoPath,
                        inputAudioPath,
                        outputVideoName,
                        shortestClip,
                        saveToCameraRoll
                };
            _javaClass.Call("mixVideoAndAudio", _params);
#endif
        }

        /// <summary>
        /// Inits the converter variables.
        /// </summary>
        /// <param name="gameObjName">The attached game object name.</param>
        /// <param name="callbackMethod">The callback method which will be called by UnitySendMessage.</param>
        /// <param name="outputVideoFile">The output video file name, set as empty string to use default name</param>
        /// <param name="audioPath">The audio path which you wnat to attach, if you don't want to attach the audio file, set as empty string</param>
        /// <param name="width">The video width.</param>
        /// <param name="height">The video height.</param>
        /// <param name="frameRate">The video frame rate.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        public void InitVars(String gameObjName, String callbackMethod, String outputVideoFile, String audioPath, int width, int height, int frameRate, bool shortestClip, bool saveToCameraRoll)
        {
#if UNITY_IOS
            InitConverter(gameObjName, callbackMethod, outputVideoFile, audioPath, width, height, frameRate, shortestClip, saveToCameraRoll);
#elif UNITY_ANDROID
            object[] _params = {
            gameObjName,
            callbackMethod,
            outputVideoFile,
            audioPath,
            width,
            height,
            frameRate,
            shortestClip,
            saveToCameraRoll
        };
            //     Debug.Log("params => " + _params);
            _javaClass.Call("initVars", _params);
#endif
        }

#if UNITY_ANDROID
        /// <summary>
        /// Inits the converter variables.
        /// </summary>
        /// <param name="gameObjName">The attached game object name.</param>
        /// <param name="callbackMethod">The callback method which will be called by UnitySendMessage.</param>
        /// <param name="outputVideoFile">The output video file name, set as empty string to use default name</param>
        /// <param name="audioPath">The audio path which you wnat to attach, if you don't want to attach the audio file, set as empty string</param>
        /// <param name="recordType">The audio recording type, from game, mic or file</param> 
        /// <param name="width">The video width.</param>
        /// <param name="height">The video height.</param>
        /// <param name="frameRate">The video frame rate.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="texturePtr">RenderTexture identifier.</param>
        /// <param name="colorBufferPtr">Color render buffer identifier.</param>
        /// <param name="depthBufferPtr">Depth render buffer identifier.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        public void InitVarsWithTextureId(String gameObjName, String callbackMethod, String outputVideoFile, String audioPath, RecordType recordType, int width, int height, int frameRate, bool shortestClip, IntPtr texturePtr, IntPtr colorBufferPtr, IntPtr depthBufferPtr, bool saveToCameraRoll)
        {

            object[] _params = { outputVideoFile };
            string outputPath = _javaClass.Call<string>("getVideoOutputPath", _params);

            GL.IssuePluginEvent(InitVideoConverterWithTextureId(gameObjName, callbackMethod, outputPath, audioPath, recordType, width, height, frameRate, shortestClip), (int)Consts.RenderEventId.CT_GLINITCONVERTER);
            GL.IssuePluginEvent(BindTexture(texturePtr, colorBufferPtr, depthBufferPtr), (int)Consts.RenderEventId.CT_GLBINDTEXTURE);

            object[] _ivar = { saveToCameraRoll };
            _javaClass.Call("convertingRenderTextureData", _ivar);//Prepare to convert
        }
#endif

#if UNITY_IOS
        /// <summary>
        /// Inits the convert variables
        /// </summary>
        /// <param name="gameObjName">The attached game object name.</param>
        /// <param name="callbackMethod">The callback method which will be called by UnitySendMessage.</param>
        /// <param name="outputVideoFile">The output video file name, set as empty string to use default name</param>
        /// <param name="audioPath">The audio path which you wnat to attach, if you don't want to attach the audio file, set as empty string</param>
        /// <param name="width">The video width.</param>
        /// <param name="height">The video height.</param>
        /// <param name="frameRate">The video frame rate.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="colorBuffer">The color framebuffer.</param>
        /// <param name="depthBuffer">The depth framebuffer.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        public void InitVarsWithBuffers(String gameObjName, String callbackMethod, String outputVideoFile, String audioPath, int width, int height, int frameRate, bool shortestClip, IntPtr texturePtr, IntPtr colorBuffer, IntPtr depthBuffer, bool saveToCameraRoll)
        {

            GL.IssuePluginEvent(InitConverterWithBuffers(gameObjName, callbackMethod, outputVideoFile, audioPath, width, height, frameRate, shortestClip, saveToCameraRoll), (int)Consts.RenderEventId.CT_GLINITCONVERTER);
            SetRenderBuffers(texturePtr, colorBuffer, depthBuffer);
        }
#endif

        /// <summary>
        /// Unbind the texture and set the texture id as 0
        /// </summary>
        public void UnboundTexture()
        {
#if UNITY_ANDROID
            GL.IssuePluginEvent(UnBindTexture(), (int)Consts.RenderEventId.CT_GLUNBINDTEXTURE);
#elif UNITY_IOS
#endif
        }

        /// <summary>
        /// Converts the image to video.
        /// </summary>
        /// <param name="bytes">The images data bytes array.</param>
        /// <param name="frameIndicator">The image frame indicator which indicate the position of the image in the video.</param>
        public void ConvertImageToVideo(byte[] bytes, int frameIndicator)
        {
            StartCoroutine(Converting(bytes, frameIndicator));
        }

        /// <summary>
        /// Converts the render texture to video.
        /// </summary>
        /// <param name="frameIndicator">Frame indicator.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void ConvertFrameBufferToVideo(int frameIndicator, int width, int height, int fps)
        {
#if UNITY_ANDROID
            GL.IssuePluginEvent(EncodeVideoWithNativeTextureId(frameIndicator, fps), (int)Consts.RenderEventId.CT_GLENCODETEXTURE);
#elif UNITY_IOS
            GL.IssuePluginEvent(EncodeVideoWithBuffers(frameIndicator, fps), (int)Consts.RenderEventId.CT_GLENCODEFRAMEBUFFER);
#endif
        }

        // public void ConvertFramebufferToVideo(int frameIndicator, int width, int height) {
        //     GL.IssuePluginEvent(GetRenderEventFunc(), (int)RenderEventId.CT_GLENCODEFRAMEBUFFER);
        // }

        /// <summary>
        /// Converting images finished, and after generating the video, the callback method which the developer pass through InitVars will be trigger.
        /// </summary>
        public void ConvertImagesFinished()
        {
#if UNITY_IOS
            FinishedEncodingVideo();
#elif UNITY_ANDROID
            _javaClass.Call("finishConverting");
#endif
        }

        /// <summary>
        /// Convertings the render texture finished.
        /// </summary>
        public void ConvertingTextureFinished()
        {
#if UNITY_ANDROID
            GL.IssuePluginEvent(UnBindTexture(), (int)Consts.RenderEventId.CT_GLUNBINDTEXTURE);
            GL.IssuePluginEvent(FinishVideoConverterWithTextureId(), (int)Consts.RenderEventId.CT_GLFINISHCONVERTER);
#elif UNITY_IOS
            GL.IssuePluginEvent(FinishVideoConverterWithBuffers(), (int)Consts.RenderEventId.CT_GLFINISHCONVERTER);
#endif
        }

        public void InterruptEncoding()
        {
#if UNITY_ANDROID
            GL.IssuePluginEvent(UnBindTexture(), (int)Consts.RenderEventId.CT_GLUNBINDTEXTURE);
            GL.IssuePluginEvent(InterruptEncodeVideo(), (int)Consts.RenderEventId.CT_GLINTERRUPTENCODE);
#elif UNITY_IOS
            GL.IssuePluginEvent(InterruptEncodeVideo(), (int)Consts.RenderEventId.CT_GLINTERRUPTENCODE);
#endif
        }
        /// <summary>
        /// Converting the data byte arraywith frame indicator.
        /// </summary>
        /// <param name="bytes">Data bytes.</param>
        /// <param name="indicator">Frame indicator.</param>
        IEnumerator Converting(byte[] bytes, int indicator)
        {
#if UNITY_IOS
            EncodeVideoData(bytes, bytes.Length, indicator);
#elif UNITY_ANDROID
            object[] _params = { bytes, indicator };
            _javaClass.Call("converting", _params);
#endif
            yield return null;
        }
        public void DisplayProgress(String title, String message)
        {
            /* Display progress */
#if UNITY_IOS
            //Nothing to do!
#elif UNITY_ANDROID
            object[] _params = { title, message };
            _javaClass.Call("displayProgressDialog", _params);
#endif
        }

        public void DismissProgress()
        {
#if UNITY_IOS
            //Nothing to do
#elif UNITY_ANDROID
            _javaClass.Call("dismissProgressDialog");
#endif
        }

        public void DisplayMessage(String message)
        {
#if UNITY_IOS
            DisplayAlertView(message);
#elif UNITY_ANDROID
            object[] _params = { message };
            _javaClass.Call("displayToastMessage", _params);
#endif
        }

#if UNITY_ANDROID
        public string[] GetAssetsContentList(String folder, String ext)
        {
            object[] _params = { folder, ext };
            return _javaClass.Call<string[]>("getAssetsContentList", _params);
        }
#endif

        /// <summary>
        /// Prepares the record from mic.
        /// </summary>
        public void PrepareRecordFromMic()
        {
#if UNITY_IOS
            PrepareForRecordingFromMic();
#elif UNITY_ANDROID
            
#endif
        }
        /// <summary>
        /// Start record from microphone by calling native API
        /// </summary>
        /// <param name="sampleRate">The audio sample rate</param>
        public void StartRecordFromMicrophone(int sampleRate)
        {
#if UNITY_IOS
            StartRecordFromMic(sampleRate);
#elif UNITY_ANDROID
            _javaClass.Call("startRecord");
#endif
        }

        public void StopRecordFromMicrophone()
        {
#if UNITY_IOS
            StopRecordFromMic();
#elif UNITY_ANDROID
            _javaClass.Call("stopRecord");
#endif
        }

        public void RegisterSystemVolumeNotification(String gameObjName, String callbackMethod)
        {
#if UNITY_IOS
            RegisterSystemVolumeChangeNotification(gameObjName, callbackMethod);
#elif UNITY_ANDROID
            object[] _params = {
            gameObjName,
            callbackMethod,
            };
            _javaClass.Call("registerSystemVolumeChangeNotification", _params);
#endif
        }

        public void UnregisterSystemVolumeNotification()
        {
#if UNITY_IOS
            UnregisterSystemVolumeChangeNotification();
#elif UNITY_ANDROID
            _javaClass.Call("unregisterSystemVolumeChangeNotification");
#endif
        }

        /// <summary>
        /// Save the temporary output video to camera roll manually.
        /// </summary>
        /// <param name="gameObjName">Attached object name.</param>
        /// <param name="callbackMethod">Callback method name.</param>
        /// <param name="outputVideoFile">Converted temporary video file path</param>
        public void SaveToCameraRoll(String gameObjName, String callbackMethod, String outputVideoFile)
        {
#if UNITY_IOS
            SaveToAlbum(gameObjName, callbackMethod, outputVideoFile);
#elif UNITY_ANDROID
            object[] _params = {
                gameObjName,
                callbackMethod,
                outputVideoFile
            };
            _javaClass.Call("saveToCameraRoll", _params);
#endif
        }

        /// <summary>
        /// Share the video to SNS
        /// </summary>
        /// <param name="type">SNS type</param>
        /// <param name="filePath">The video file path</param>
        public void ShareVideoToSNS(SNSType type, String filePath)
        {
#if UNITY_IOS
            ShareToSNS(type, filePath);
#elif UNITY_ANDROID
#endif
        }
#if UNITY_IOS
        [DllImport("__Internal")]
        /// <summary>
        /// Inits the converter.
        /// </summary>
        /// <param name="obj">Attached object</param>
        /// <param name="method">Callback method name</param>
        /// <param name="videoName">Video name</param>
        /// <param name="audioPath">Audio path</param>
        /// <param name="width">Captured Image Width</param>
        /// <param name="height">Captured Image Height</param>
        /// <param name="fps">Fps</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        private static extern void InitConverter(string obj, string method, string videoName, string audioPath, int width, int height, int fps, bool shortestClip, bool saveToCameraRoll);

        [DllImport("__Internal")]
        /// <summary>
        /// Encodes the video data.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        /// <param name="byteLength">Byte length.</param>
        /// <param name="frameIndicator">Frame indicator.</param>
        private static extern void EncodeVideoData(byte[] bytes, int byteLength, int frameIndicator);

        [DllImport("__Internal")]
        /// <summary>
        /// Finish to encode the video.
        /// </summary>
        private static extern void FinishedEncodingVideo();

        [DllImport("__Internal")]
        private static extern void DisplayAlertView(string message);

        // /// <summary>
        // /// Setup capture framebuffers.
        // /// </summary>
        // [DllImport("__Internal")]
        // private static extern void SetCaptureBuffers(IntPtr colorBuffer, IntPtr depthBuffer);

        /// <summary>
        /// Setup render framebuffers.
        /// </summary>
        /// <param name="texturePtr"></param>
        /// <param name="colorBuffer"></param>
        /// <param name="depthBuffer"></param>
        [DllImport("__Internal")]
        private static extern void SetRenderBuffers(IntPtr texturePtr, IntPtr colorBuffer, IntPtr depthBuffer);

        /// <summary>
        /// Inits the converter.
        /// </summary>
        /// <param name="obj">Attached object</param>
        /// <param name="method">Callback method name</param>
        /// <param name="videoName">Video name</param>
        /// <param name="audioPath">Audio path</param>
        /// <param name="width">Captured Image Width</param>
        /// <param name="height">Captured Image Height</param>
        /// <param name="fps">Fps</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        /// <returns>Initialize converter identifier.</returns>
        [DllImport("__Internal")]
        private static extern IntPtr InitConverterWithBuffers(string obj, string method, string videoName, string audioPath, int width, int height, int fps, bool shortestClip, bool saveToCameraRoll);

        /// <summary>
        /// Encode framebuffer data to the video.
        /// </summary>
        /// <param name="frameIndex">Frame index</param>
        /// <param name="fps">Current frame rate per second</param>
        /// <returns>Encoding texture identifier.</returns>
        [DllImport("__Internal")]
        private static extern IntPtr EncodeVideoWithBuffers(int frameIndex, int fps);

        /// <summary>
        /// Finish encoding framebuffer data to the video.
        /// </summary>
        /// <returns>Finishing encoding texture identifier.</returns>
        [DllImport("__Internal")]
        private static extern IntPtr FinishVideoConverterWithBuffers();

        /// <summary>
        /// Interrupt current encoding process and clean the rest queued frames.
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern IntPtr InterruptEncodeVideo();

        /// <summary>
        /// Prepares for recording from mic.
        /// </summary>
        /// <returns>The for recording from mic.</returns>
        [DllImport("__Internal")]
        private static extern IntPtr PrepareForRecordingFromMic();

        /// <summary>
        /// Start record from microphone
        /// </summary>
        /// <param name="sampleRate">The audio sample rate</param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern IntPtr StartRecordFromMic(int sampleRate);

        /// <summary>
        /// Stop record from microphone
        /// </summary>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern IntPtr StopRecordFromMic();

        /// <summary>
        /// Mix the mp4 video and the mp3/pcm audio to the mp4 video
        /// </summary>
        /// <param name="obj">Attached object.</param>
        /// <param name="method">Callback method name.</param>
        /// <param name="inputVideo">The input video file path.</param>
        /// <param name="inputAudio">The input audio file path.</param>
        /// <param name="outputVideo">The output video file name.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        /// <param name="saveToCameraRoll">If set to <c>true</c> save to camera roll.</param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern IntPtr MixVideoAndAudio(string obj, string method, string inputVideo, string inputAudio, string outputVideo, bool shortestClip, bool saveToCameraRoll);

        /// <summary>
        /// Save the video to gallery mannually
        /// </summary>
        /// <param name="obj">Attached object.</param>
        /// <param name="method">Callback method name.</param>
        /// <param name="outputVideo">The output video file name.</param>
        /// <returns>save camera roll identifier.</returns>
        [DllImport("__Internal")]
        private static extern IntPtr SaveToAlbum(string obj, string method, string outputVideo);

        [DllImport("__Internal")]
        private static extern IntPtr RegisterSystemVolumeChangeNotification(string obj, string method);

        [DllImport("__Internal")]
        private static extern IntPtr UnregisterSystemVolumeChangeNotification();

        /// <summary>
        /// Share the video to SNS
        /// </summary>
        /// <param name="type">SNS type, such as WhatsApp, Line, WeChat or SMS</param>
        /// <param name="filePath">The video file path</param>
        /// <returns></returns>
        [DllImport("__Internal")]
        private static extern IntPtr ShareToSNS(SNSType type, string filePath);
#elif UNITY_ANDROID
        /// <summary>
        /// Inits the video converter with render texture identifier.
        /// </summary>
        /// <returns>The video converter with render texture identifier.</returns>
        /// <param name="callbackMethod">The callback method which will be called by UnitySendMessage.</param>
        /// <param name="outputVideoFile">The output video file name, set as empty string to use default name</param>
        /// <param name="audioPath">The audio path which you wnat to attach, if you don't want to attach the audio file, set as empty string</param>
        /// <param name="recordType">The audio recording type, from game, mic or file</param>
        /// <param name="width">The video width.</param>
        /// <param name="height">The video height.</param>
        /// <param name="frameRate">The video frame rate.</param>
        /// <param name="shortestClip">If set to <c>true</c> shortest clip.</param>
        [DllImport("videoconverter")]
        private static extern IntPtr InitVideoConverterWithTextureId(
            string objName, string methodName,
            string outputFile, string audioFile, RecordType recordType,
            int width, int height, int fps, bool shortClip);


        /// <summary>
        /// Binds the render texture identifier.
        /// </summary>
        /// <returns>The render texture.</returns>
        /// <param name="texture">RenderTexture identifier.</param>
        [DllImport("videoconverter")]
        private static extern IntPtr BindTexture(IntPtr texture, IntPtr colorBufferPtr, IntPtr depthBufferPtr);

        /// <summary>
        /// Unbound the render texture.
        /// </summary>
        /// <returns>The bind render texture.</returns>
        [DllImport("videoconverter")]
        private static extern IntPtr UnBindTexture();

        /// <summary>
        /// Encodes the video with render texture identifier.
        /// </summary>
        /// <returns>The render texture identifier.</returns>
        /// <param name="index">Frame index.</param>
        /// <param name="fps">Frame per second</param>
        [DllImport("videoconverter")]
        private static extern IntPtr EncodeVideoWithNativeTextureId(int index, int fps);

        /// <summary>
        /// Finishs the video converter with render texture identifier.
        /// </summary>
        /// <returns>The video converter with render texture identifier.</returns>
        [DllImport("videoconverter")]
        private static extern IntPtr FinishVideoConverterWithTextureId();

        /// <summary>
        /// Interrupt current encoding process and clean the rest queued frames.
        /// </summary>
        /// <returns></returns>
        [DllImport("videoconverter")]
        private static extern IntPtr InterruptEncodeVideo();
#endif
    }
}