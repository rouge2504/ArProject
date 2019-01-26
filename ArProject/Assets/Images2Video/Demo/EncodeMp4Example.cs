using UnityEngine;
using tw.com.championtek;

public class EncodeMp4Example : MonoBehaviour
{

    public void encodeMp4()
    {
        //Prepare for encoding
        GameObject mp4Encdoer = GameObject.Find("MP4Encoder");
        MP4Encoder mp4Obj = (MP4Encoder)mp4Encdoer.GetComponent("MP4Encoder");
        mp4Obj.EncodeVideo();
    }

    public void mixMp4()
    {
        GameObject mixEncoder = GameObject.Find("MixMP4");
        MixMP4 mixObj = (MixMP4)mixEncoder.GetComponent("MixMP4");
        mixObj.MixVideoAndAudio();
    }
}
