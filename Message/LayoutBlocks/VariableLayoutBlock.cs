using System;
using System.Linq;

namespace TCPCommunication.Message
{
    public class VariableLayoutBlock<T> : LayoutBlock<T> where T : IHeaderBytes<T>, new()
    {
        public override int BlockSize { get; protected set; }

        public override T Read(byte[] headerBytes, int startIdx)
        {
            var arraySize = BitConverter.ToUInt16(headerBytes, startIdx);
            BlockSize = arraySize + 2;
            return new T().ToValue(headerBytes, 2 + startIdx);
        }

        public override void Write(T value)
        {
            var valueBytes = value.GetBytes();
            var sizeBytes = BitConverter.GetBytes((ushort) valueBytes.Length);
            bytes = sizeBytes.Concat(valueBytes).ToArray();
            BlockSize = bytes.Length;
        }
    }
}