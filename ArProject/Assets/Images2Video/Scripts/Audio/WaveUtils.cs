using System;
using System.IO;

namespace tw.com.championtek.Audio
{
    public class WaveUtils {
        private BinaryWriter writer;
        private string filePath;
        private int sampleRate;
        private WaveHeader header;
        private WaveFormatChunk format;
        private WaveDataChunk data;
        public WaveUtils(string filePath, int sampleRate) {
            this.filePath = filePath;
            this.sampleRate = sampleRate;
        }

        /// <summary>
        /// Create the empty header with zero byte
        /// </summary>
        public void CreateEmptyHeader()
        {  
            var stream = new FileStream(filePath, FileMode.Create);
            writer = new BinaryWriter(stream);

            for (int i = 0; i < Consts.HeaderSize; i++)
                writer.Write((byte)0);

            header = new WaveHeader();
            format = new WaveFormatChunk();
            data = new WaveDataChunk();
        }

        public void WriteHeader()
        {
            var closeWriter = writer;
            writer = null;

            long pos = closeWriter.BaseStream.Length;
            closeWriter.Seek(0, SeekOrigin.Begin);

            /// Write the header data
            closeWriter.Write(header.groupID.ToCharArray());

            header.fileLength = (uint)(pos - 8);
            closeWriter.Write(header.fileLength);

            closeWriter.Write(header.riffType.ToCharArray());

            ///Write the format chunk
            closeWriter.Write(format.chunkID.ToCharArray());
            closeWriter.Write(format.chunkSize);
            closeWriter.Write(format.formatTag);
            closeWriter.Write(format.channels);

            //Reset the sampe rate
            format.samplePerSec = (uint)sampleRate;
            closeWriter.Write(format.samplePerSec);
            //Reset average bytes per second
            format.avgBytesPerSec = format.samplePerSec * format.blockAlign;
            closeWriter.Write(format.avgBytesPerSec);
            closeWriter.Write(format.blockAlign);
            closeWriter.Write(format.bitsPerSample);

            //Write the data chunk
            closeWriter.Write(data.chunkID.ToCharArray());
            data.chunkSize = (uint)(pos - Consts.HeaderSize);
            closeWriter.Write(data.chunkSize);
            closeWriter.Seek((int)pos, SeekOrigin.Begin);

            closeWriter.Flush();
            closeWriter.Close();
        }

        public void ConvertAndWrite(Byte[] data)
        {
            if (writer == null)
                return;
            writer.Write(data);
        }

        public void ReleaseResources()
        {
            header = null;
            format = null;
            data = null;
        }
    }
}