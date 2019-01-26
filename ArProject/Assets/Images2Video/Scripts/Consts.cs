namespace tw.com.championtek
{
    [System.Serializable]
    public class Consts
    {
        /// <summary>
        /// The render event id for issuing plugin event
        /// </summary>
        public enum RenderEventId
        {
            CT_GLINITCONVERTER = 9910,
            CT_GLENCODETEXTURE = 9911,
            CT_GLFINISHCONVERTER = 9912,
            CT_GLBINDTEXTURE = 9913,
            CT_GLUNBINDTEXTURE = 9914,
            CT_GLENCODEFRAMEBUFFER = 9915,
            CT_GLINTERRUPTENCODE = 9920
        };

        /// <summary>
        /// WAVE file format header size.
        /// </summary>
        public const int HeaderSize = 44;
        /// <summary>
        /// WAVE rescaler factor
        /// </summary>
        public const float Rescale_Factor = 32767;

    }

    /// <summary>
    /// Render texture size to generate the same video size
    /// </summary>
    public enum VideoDimension
    {
        RenderTextureSize,
        MainCameraSize
    }
    /// <summary>
    /// Recording type for virtual camera object
    /// </summary>
    public enum RecordType
    {
        None = 0,
        InGames = 1,
        FromMic = 2,
        FromPlugin = 4,
    }

    public enum StopType
    {
        Stop = 0,
        Interrupt = 1
    }

    public enum SNSType
    {
        WhatsApp = 0,
        LINE = 1,
        WeChat = 2,
        SMS = 3
    }
}
