using System.Runtime.InteropServices;

namespace EarTraining.Classes
{
    /// <summary>WAV audio file 'riff' section header</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WavRiff
    {
        public const string RiffStr = "RIFF";
        public const string WaveStr = "WAVE";
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)] 
        public char[] Riff;
        public int PackageLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)] 
        public char[] Wave;
    }
}