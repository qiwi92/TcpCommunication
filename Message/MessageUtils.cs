using System;
using System.Linq;
using System.Text;

namespace TCPCommunication.Message
{
    public static class MessageUtils
    {
        public static byte[] GetBytes(byte value) => new[] {value};
        public static byte ToByte(byte[] header, int startIdx) => header[startIdx];

        public static byte[] GetBytes(ushort value) => BitConverter.GetBytes(value);
        public static ushort ToUshort(byte[] header, int startIdx) => BitConverter.ToUInt16(header, startIdx);

        public static byte[] GetBytes(uint value) => BitConverter.GetBytes(value);
        public static uint ToUint(byte[] header, int startIdx) => BitConverter.ToUInt32(header, startIdx);

        public static byte[] GetBytes(int value) => BitConverter.GetBytes(value);
        public static int ToInt(byte[] bytes, int idx) => BitConverter.ToInt32(bytes, idx);
        
        public static byte[] GetBytes(float value) => BitConverter.GetBytes(value);
        public static float ToFloat(byte[] header, int startIdx) => BitConverter.ToSingle(header, startIdx);

        public static byte[] GetBytes(string value)
        {
            var stringBytes = Encoding.UTF8.GetBytes(value);
            var stringBytesLength = stringBytes.Length;
            
            if(stringBytesLength > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value) + " is not serializable");
            
            var lengthBytes = GetBytes((ushort) stringBytesLength);
            return lengthBytes.Concat(stringBytes).ToArray();
        }

        public static string ToString(byte[] header, int startIdx, out int stringLength)
        {
            stringLength = ToUshort(header, startIdx);
            return Encoding.UTF8.GetString(header, startIdx + 2, stringLength);
        }
    }
}