using System;
using System.Globalization;
using System.Linq;

namespace Common
{
    public class MacHelper
    {
        public static string ConvertBytesToMac(byte[] mac)
        {
            if (mac == null)
                return "";
            // Array.Reverse(mac);
            var dotAfterTwoChar = string.Join(".", (from z in mac select z.ToString("X2")).ToArray());
           var result= dotAfterTwoChar.Where((ch, index) =>! (((index % 3) == 2 )&& ((index % 6) == 2)));
           return new string(result.ToArray());
        }

        public static byte[] ConvertMacToBytes(string mac)
        {
            string rawMac = mac.Replace(".", "").Replace(":", "");
            long value = long.Parse(rawMac, NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat);
            byte[] macBytes = BitConverter.GetBytes(value);
            Array.Reverse(macBytes);
            byte[] macAddress = new byte[6];
            for (int i = 0; i <= 5; i++)
                macAddress[i] = macBytes[i + 2];
            return macAddress;
        }
    }
}
