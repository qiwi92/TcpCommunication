using System;

namespace TCPCommunication.Message
{
    class DateTimeLayoutBlock : LayoutBlock<DateTime>
    {
        public override int BlockSize { get; protected set; } = 6;

        public override DateTime Read(byte[] headerBytes, int startIdx)
        {
            return new DateTime(
                headerBytes[startIdx + 5] + 2000,
                headerBytes[startIdx + 4],
                headerBytes[startIdx + 3],
                headerBytes[startIdx + 2],
                headerBytes[startIdx + 1],
                headerBytes[startIdx]);
        }

        public override void Write(DateTime value)
        {
            bytes = new byte[6];

            bytes[0] = (byte) value.Second;
            bytes[1] = (byte) value.Minute;
            bytes[2] = (byte) value.Hour;
            bytes[3] = (byte) value.Day;
            bytes[4] = (byte) value.Month;
            bytes[5] = (byte) (value.Year % 100);
        }
    }
}