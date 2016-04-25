namespace EarTraining.Classes
{
    internal sealed class BigEndianHelper : EndianHelper
    {

        public override int Swap32(ref int dwData)
        {
            var data = unchecked((uint)dwData);
            dwData = unchecked((int) (((data >> 24) & 0x000000FF) |
                                      ((data >> 8) & 0x0000FF00) |
                                      ((data << 8) & 0x00FF0000) |
                                      ((data << 24) & 0xFF000000)));
            return dwData;
        }

        public override short Swap16(ref short wData)
        {
            wData = unchecked((short)(((wData >> 8) & 0x00FF) |
                                      ((wData << 8) & 0xFF00)));
            return wData;
        }

        public override void Swap16Buffer(short[] pData, int dwNumWords)
        {
            for (int i = 0; i < dwNumWords; i++)
            {
                pData[i] = Swap16(ref pData[i]);
            }
        }
    }
}