using System;

namespace InoEEPROMProgrammer
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the Nth byte of given integer (by right-moving it and trimming)
        /// </summary>
        /// <param name="value">source value</param>
        /// <param name="byteNumber">index of byte in int</param>
        public static byte GetNthByte(this int value, int byteNumber)
        {
            if(byteNumber > 3)
                throw new ArgumentOutOfRangeException("Int32 has only 4 bytes!");

            return (byte)((value >> (byteNumber * 8)) & 0xFF);
        }

        public static byte[] ToBytes(this int value)
        {
            return new[] {
                (byte)(value & 0xFF),
                (byte)((value >> 8) & 0xFF),
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 24) & 0xFF),
            };
        }

        public static byte ToByte(this string value)
        {
            if(value.StartsWith("0x") || value.StartsWith("x"))
            {
                return Convert.ToByte(value.TrimStart('0').TrimStart('x'), 16);
            }
            else 
            {
                return Convert.ToByte(value);
            }
        }
    }
}