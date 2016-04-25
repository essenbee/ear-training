using System.Runtime.InteropServices;

namespace EarTraining.Classes
{
    /// <summary>WAV audio file 'data' section header</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WavData
    {
        public const string DataStr = "data";
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)] 
        public char[] DataField;
        public int DataLen;
    }
}