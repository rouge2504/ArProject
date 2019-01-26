# Images2Video README

## Description

The best solution on Android and iOS platforms for merging serial images into a video file and capturing texture in realtime and merge them into a video file!
By generating a mp4 format video which is almost universal, no additional software required. Also you can import the mp3 or PCM file while merging the video file.
It's developed for iOS and Android. It's a Unity runtime package, not a Unity editor package.

## Requirement

* For iOS requires SDK 8.1 or higher
* For Android requires API 19 or higher

## Features

* The video file format is ```H264 mp4``` file.
* On Android, use ```OpenGL ES 2.0|3.0``` to fetch the render texture data.
* On iOS, use ```Metal|OpenGL ES 3.0``` to fetch the render texture data.
* Implement by native API to record audio.

## Demo scenes

Three simple scenes are included with sample scripts demonstrating its functionality.

* ScreenshotExample
* MP4EncoderExample
* VirtualCamera
* VuforiaExample

## Support

If you have any questions, please send to <service@championtek.com.tw>.

## Document

Please check more detail in [README.pdf](./README.pdf).

## FAQ

## 1. All of these examples crash

If you create the new project and import one of these three examples. Before run your application, please make sure,

1. ```Player Settings > Other Settings > Write permission``` is set as ```External(SD Card)```.
2. ```Graphic APIs``` is set as
    * ```OpenGL ES 2.0|3.0``` for Android
    * ```OpenGL ES 3.0|Metal``` for iOS

## 2. How to enable recording from microphone

Please goto ```Player Settings > Other Settings > Scripting Define Symbols``` and add ```ENABLE_MIC```, then rebuild the application.

## Change logs

* 1.0.1 - Improve the performance and fix the bugs
* 1.0.2 - Add document and README files
* 1.0.3 - Fix the issue which can't generate the video file for Android Lollipop
* 1.0.4 - Test and upload the on Unity 5.1.0f3 Personal
* 1.5.0 - Support importing the audio file to the video file.
* 1.5.1 - Fix the video can't save to album on iPad.
* 1.5.2 - Update the iOS library.
* 1.5.3 - Add input file extension parameter to support merging jpg images to the video file.
* 1.5.4 - Update frameRate
* 1.5.5 - Fix bugs
* 1.5.6 - Fix bugs
* 1.5.7 - Change the process which download audio file from server
* 1.5.8 - Fix bugs
* 1.5.9 - Fix bugs
* 1.6.0 - Change Android plugin jar file to aar file.
* 1.6.1 - Fix the bug while generating the video without audio.
* 1.6.2 - Update Screenshot Example and use the render texture to render the screen.
* 1.6.3 - Fix bugs.
* 1.6.4 - Remove AndroidManifest.xml.
* 2.0.0 - Rewrite the whole Android plugin and improve the encoding performance on Android and fix bugs.
* 2.0.1 - Fix the latency issue of the converted video.
* 2.1.0 - Add two new example scenes to demonstrate how to merge images to the video and render screenshots then merge images to the video.
* 2.1.1 - Support PCM format file.
* 2.1.2 - Bugs fixed.
* 2.1.3 - Fix memory leaks.
* 2.1.4 - Fix bugs and add armeabi architecture folder.
* 2.2.0 - Support for recording in games or from microphone. Fix Xcode bitcode issue. Fix bugs.
* 2.2.1 - Fix bugs. Support the feature for mix audio file and video file.
* 2.2.2 - Fix bugs. Correct the dimension of texture to make sure generate the valid video.
* 2.2.3 - Fix bugs. Check the video extension name to make sure encoder can identify the format.
* 2.2.4 - Update platform dependencies to prevent from compiling error.
* 2.2.5 - Fix bugs.
* 2.2.6 - Fix the issue which can not save to camera roll on Android.
* 2.2.7 - Fix bugs.
* 2.2.8 - Add apative fps feature to correct the frame rate per second for converting video.
* 2.2.9 - Add the new feature for syncing with the volume of the audio source and system volume while using virtual camera.
* 2.2.10 - Fix bugs. Update interrupt process.
* 3.0.0 - Support OpenGL ES 3.0 on Android.
* 3.0.1 - Add SaveToCameraRoll API. Now you can save the video to camera roll manually.
* 3.0.2 - Add duration property for converting the video.
* 3.0.3 - Fix bugs.
* 3.0.4 - Check the width of texture can be divided by 16.
* 3.0.5 - Fix bugs and upgrade to 2017.1.1.f1.
* 3.0.6 - Fix the black frames at the begin of the converted video on iOS.
* 3.0.7 - Fix the video duration start at 0 second.
* 3.0.8 - Support to specify the output video path on Android platform.
* 4.0.0 - Support OpenGL ES 3.0 on iOS and Samsung devices.
* 4.0.1 - Fix bugs.
* 4.0.2 - Add Vuforia demo example.
* 4.0.3 - Fix bugs.
* 4.0.4 - Fix execute on iOS device will crash.
* 4.1.0 - Improve the performance for memory copy operation on Android.

&copy;2018 Champ Info Co., Ltd. &reg;
