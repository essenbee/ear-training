using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace EarTraining.Classes
{
    public static class ExtensionMethods
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static int Read(this Stream stream, out string value, int count)
        {
            var data = new byte[count];
            int read = stream.Read(data, 0, count);
            value = Encoding.ASCII.GetString(data, 0, read);
            return read;
        }

        public static int Read(this Stream stream, out int value)
        {
            var data = new byte[sizeof(int)];
            int read = stream.Read(data, 0, sizeof(int));
            value = BitConverter.ToInt32(data, 0);
            return read;
        }

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
