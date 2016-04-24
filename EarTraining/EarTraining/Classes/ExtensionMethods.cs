using System.Collections.Generic;

namespace EarTraining.Classes
{
    public static class ExtensionMethods
    {

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            var rng = new CryptoRandom();
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
