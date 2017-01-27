using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public static class HexConverter
    {
        private static uint[] halfs;
        private static char[] chars;

        static HexConverter()
        {
            halfs = new uint[121];

            for (int i = 0; i < 121; i++)
            {
                halfs[i] = 0x1000U;
            }

            halfs['0'] = 0x00U;
            halfs['1'] = 0x01U;
            halfs['2'] = 0x02U;
            halfs['3'] = 0x03U;
            halfs['4'] = 0x04U;
            halfs['5'] = 0x05U;
            halfs['6'] = 0x06U;
            halfs['7'] = 0x07U;
            halfs['8'] = 0x08U;
            halfs['9'] = 0x09U;

            halfs['a'] = 0x0AU;
            halfs['b'] = 0x0BU;
            halfs['c'] = 0x0CU;
            halfs['d'] = 0x0DU;
            halfs['e'] = 0x0EU;
            halfs['f'] = 0x0FU;

            halfs['A'] = 0x0AU;
            halfs['B'] = 0x0BU;
            halfs['C'] = 0x0CU;
            halfs['D'] = 0x0DU;
            halfs['E'] = 0x0EU;
            halfs['F'] = 0x0FU;

            halfs['x'] = 0x0100U;
            halfs['X'] = 0x0100U;

            chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        }

        public static byte[] GetBytes(string hex)
        {
            int i = 0;
            int j = 0;
            int c;
            uint half1;
            uint half2;
            byte[] buff = new byte[hex.Length / 2];
            while (i < hex.Length - 1)
            {
                if ((c = hex[i++]) > 120)
                {
                    continue;
                }

                if ((half1 = halfs[c]) > 0x0FU)
                {
                    continue;
                }

                if ((c = hex[i++]) > 120)
                {
                    throw new ArgumentException("Input string was invalid", nameof(hex));
                }

                if ((half2 = halfs[c]) == 0x0100U)
                {
                    continue;
                }

                if (half2 == 0x1000U)
                {
                    throw new ArgumentException("Input string was invalid", nameof(hex));
                }

                buff[j++] = (byte)((half1 << 4) | half2);
            }

            if (i == hex.Length - 1 && (c = hex[i]) < 121 && halfs[c] < 0x10U)
            {
                throw new ArgumentException("Input string was invalid", nameof(hex));
            }

            byte[] result = new byte[j];
            Buffer.BlockCopy(buff, 0, result, 0, j);

            return result;
        }

        public static string GetHex(byte[] bytes, string separator = null)
        {
            if (bytes.Length == 0)
            {
                return "";
            }

            bool sep = !string.IsNullOrEmpty(separator);

            StringBuilder sb = new StringBuilder();
            sb.Append(chars[bytes[0] >> 4]);
            sb.Append(chars[bytes[0] & 0x0F]);
            for (int i = 1; i < bytes.Length; i++)
            {
                if (sep)
                {
                    sb.Append(separator);
                }

                sb.Append(chars[bytes[i] >> 4]);
                sb.Append(chars[bytes[i] & 0x0F]);
            }

            return sb.ToString();
        }
    }
}
