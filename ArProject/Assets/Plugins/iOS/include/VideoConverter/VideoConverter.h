//
//  VideoConverter.h
//  VideoConverter
//
//  Created by Isaac Cheng on 11/26/14.
//  Copyright (c) 2015 Champ Info Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import <AssetsLibrary/AssetsLibrary.h>

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

extern "C" void UnitySendMessage(const char *, const char *, const char *);
extern "C" void InitConverter(const char* object, const char* method, const char* videoName, const char* audioPath, int width, int height, int fps, bool shortestClip, bool saveToCameraRoll);
extern "C" void EncodeVideoData(const char* bytes, int bytesLength, int frameIndicator);
extern "C" void FinishedEncodingVideo();
extern "C" void DisplayAlertView(const char* message);
extern "C" void PrepareForRecordingFromMic();
extern "C" void StartRecordFromMic(int sampleRate);
extern "C" void StopRecordFromMic();
extern "C" void MixVideoAndAudio(const char* object, const char* method, const char* inputVideo, const char* inputAudio, const char* outputVideo, bool shortestClip, bool saveToCameraRoll);
extern "C" void RegisterSystemVolumeChangeNotification(const char* object, const char* method);
extern "C" void UnregisterSystemVolumeChangeNotification();
extern "C" void ShareToSNS(int snsType, const char* filePath);

//Save to camera roll API
extern "C" void SaveToAlbum(const char* object, const char* method, const char* videoPath);

@interface VideoConverter : NSObject

+ (VideoConverter*)sharedConverter;
- (void)initVideoConverterWithCallback:(const char*)object method:(const char*)method;
- (void)initVideoConverterWithObject:(const char*)object method:(const char*)method videoName:(const char*)videoName audioPath:(const char*)audioPath width:(int)width height:(int)height fps:(int)fps shortestClip:(BOOL)shortestClip saveToCameraRoll:(BOOL)saveToCameraRoll;
- (void)prepareForEncodingVideo;
- (void)finishedEncodingVideo;

- (AVAssetWriterInputPixelBufferAdaptor *)getPixelBufferAdaptor;
- (void)encoding:(CVPixelBufferRef)buffer frameIndex:(int)frameIndex;
- (void)updateFps:(int)fps;
- (void)startRecordFromMicrophone;
- (void)stopRecordFromMicrophone;

- (void)mixWithObject:(const char*)object method:(const char*)method inputVideo:(const char*)inputVideo inputAudio:(const char*)inputAudio outputVideo:(const char*)outputVideo shortestClip:(BOOL)shortestClip saveToCameraRoll:(BOOL)saveToCameraRoll;
- (void)registerSystemVolumeChangeNotification:(const char*)object method:(const char*)method;
- (void)unregisterSystemVolumeChangeNotification;

- (void)saveToCameraRoll:(NSString*)path;
- (void)sendMessage:(NSString*)msg callbackObject:(NSString*)callbackObject callbackMethod:(NSString*)callbackMethod;
- (void)sendMessageToUnityWhileReadyToRecordAudio:(NSString*)msg callbackMethod:(NSString*)callbackMethod;

- (void)shareToSNS:(int)snsType withFile:(const char*)filePath;

- (UIImage *)imageFromPixelBuffer:(CVPixelBufferRef)pixelBufferRef;
- (UIImage*)imageFromBytes:(void*)bytes
                bufferSize:(int)bufferSize
                     width:(int)width
                    height:(int)height;
@end

static VideoConverter* _sharedConverter = nil;

