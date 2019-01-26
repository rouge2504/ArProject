using System;

namespace tw.com.championtek.Audio
{
    public class RecordInGames
    {
        private WaveUtils utils;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">The saved audio path</param>
        /// <param name="sampleRate">The sample rate of the audio</param>
        public RecordInGames(string filePath, int sampleRate)
        {
            utils = new WaveUtils(filePath, sampleRate);
        }

        /// <summary>
        /// Start recording.
        /// </summary>
        public void StartRecording()
        {
            utils.CreateEmptyHeader();
        }

        /// <summary>
        /// Stop recording
        /// </summary>
        public void StopRecording()
        {
            utils.WriteHeader();
            utils.ReleaseResources();
        }

        public void ConvertAndWrite(Byte[] data)
        {
            utils.ConvertAndWrite(data);
        }
    }
}