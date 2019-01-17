using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using RenderHeads.Media.AVProMovieCapture;

public class RecordCamera : MonoBehaviour
{

    // Status
    private long _lastFileSize;
    private uint _lastEncodedMinutes;
    private uint _lastEncodedSeconds;
    private uint _lastEncodedFrame;

    public bool _whenRecordingAutoHideUI = true;
    public bool _showUI = true;
    public CaptureBase _movieCapture;
    public void StartRecord()
    {
        StartCapture();
    }

    private void StartCapture()
    {
        _lastFileSize = 0;
        _lastEncodedMinutes = _lastEncodedSeconds = _lastEncodedFrame = 0;
        if (_whenRecordingAutoHideUI)
        {
            _showUI = false;
        }
        if (_movieCapture != null)
        {
            _movieCapture.StartCapture();
        }
    }
}
