namespace EarTraining.Classes
{
    internal sealed class LittleEndianHelper : EndianHelper
    {
        public override int Swap32(ref int dwData)
        {
            return dwData;
        }

        public override short Swap16(ref short wData)
        {
            return wData;
        }

        public override void Swap16Buffer(short[] pData, int dwNumWords)
        {
        }
    }
}