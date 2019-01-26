namespace tw.com.championtek.Audio
{
    /// <summary>
    /// Ref : https://blogs.msdn.microsoft.com/dawate/2009/06/24/intro-to-audio-programming-part-3-synthesizing-simple-wave-audio-using-c/
    /// </summary>
    public class WaveHeader
    {
        /// <summary>
        /// RIFF
        /// </summary>
        public string groupID;
        /// <summary>
        /// Total file length minus 8, which is taken up by RIFF
        /// </summary>
        public uint fileLength;
        /// <summary>
        /// WAVE
        /// </summary>
        public string riffType;

        /// <summary>
        /// Initialize a WaveHeader object with default value
        /// </summary>
        public WaveHeader()
        {
            groupID = "RIFF";
            fileLength = 0;
            riffType = "WAVE";
        }
    }

    public class WaveFormatChunk
    {
        /// <summary>
        /// Four bytes: "fmt "
        /// </summary>
        public string chunkID;
        /// <summary>
        /// Length of header in bytes 
        /// </summary>
        public uint chunkSize;
        /// <summary>
        /// 1 (MS PCM)
        /// </summary>
        public ushort formatTag;
        /// <summary>
        /// Number of channels
        /// </summary>
        public ushort channels;
        /// <summary>
        /// Frequency of the audio in Hz... 44100
        /// </summary>
        public uint samplePerSec;
        /// <summary>
        /// For estimate RAM allocation
        /// </summary>
        public uint avgBytesPerSec;
        /// <summary>
        /// Sample frame size, in bytes
        /// </summary>
        public ushort blockAlign;
        /// <summary>
        /// Bits per sample
        /// </summary>
        public ushort bitsPerSample;

        /// <summary>
        /// Initializes a format chunk with the following properties
        /// Sample rate : 44100 Hz
        /// Channels : Stereo
        /// Bit depth : 16-bit
        /// </summary>
        public WaveFormatChunk()
        {
            chunkID = "fmt ";
            chunkSize = 16;
            formatTag = 1;
            channels = 2;
            samplePerSec = 44100;
            bitsPerSample = 16;
            blockAlign = (ushort)(channels * (bitsPerSample / 8));
            avgBytesPerSec = samplePerSec * blockAlign;
        }
    }

    public class WaveDataChunk
    {
        /// <summary>
        /// data
        /// </summary>
        public string chunkID;
        /// <summary>
        /// Length of header in bytes
        /// </summary>
        public uint chunkSize;
        /// <summary>
        /// 8-bit audio
        /// </summary>
        public short[] shortArray;

        /// <summary>
        /// Initializes a new data chunk with default values
        /// </summary>
        public WaveDataChunk()
        {
            shortArray = new short[0];
            chunkSize = 0;
            chunkID = "data";
        }
    }
}