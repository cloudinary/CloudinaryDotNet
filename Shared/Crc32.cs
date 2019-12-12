﻿namespace CloudinaryDotNet
{
    using System;

    /// <summary>
    /// Checksum generator using CRC32 algorithm.
    /// </summary>
    public static class Crc32
    {
        private static uint[] table;

        static Crc32()
        {
            uint poly = 0xedb88320;
            table = new uint[256];
            uint temp = 0;
            for (uint i = 0; i < table.Length; ++i)
            {
                temp = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((temp & 1) == 1)
                    {
                        temp = (uint)((temp >> 1) ^ poly);
                    }
                    else
                    {
                        temp >>= 1;
                    }
                }

                table[i] = temp;
            }
        }

        /// <summary>
        /// Compute checksum for a byte array.
        /// </summary>
        /// <param name="bytes">Byte array to compute CRC.</param>
        /// <returns>Computed checksum.</returns>
        public static uint ComputeChecksum(byte[] bytes)
        {
            uint crc = 0xffffffff;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)((crc & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }

            return ~crc;
        }

        /// <summary>
        /// Compute checksum for a byte array.
        /// </summary>
        /// <param name="bytes">Byte array to compute CRC.</param>
        /// <returns>Computed checksum represented as byte array.</returns>
        public static byte[] ComputeChecksumBytes(byte[] bytes)
        {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }
    }
}
