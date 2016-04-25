using System.Runtime.InteropServices;

namespace EarTraining.Classes
{
    /// <summary>WAV audio file 'format' section header</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WavFormat
    {
        public const string FmtStr = "fmt ";
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)] 
        public char[] Fmt;
        public int FormatLen;
        public short Fixed;
        public short ChannelNumber;
        public int SampleRate;
        public int ByteRate;
        public short BytePerSample;
        public short BitsPerSample;
    }
}