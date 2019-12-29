using System;

namespace TCPCommunication.Message
{ 
    class FloatArrayLayoutBlock : LayoutBlock<float[]>
    {
        const int ChunkLength = 4;
        public override int BlockSize { get; protected set; }

        public override float[] Read(byte[] headerBytes, int startIdx)
        {
            var byteArraySize = BitConverter.ToUInt16(headerBytes, startIdx);
            BlockSize = byteArraySize + 2;

            return GetValue(headerBytes, byteArraySize, startIdx + 2);
        }

        public override void Write(float[] value)
        {
            var valueBytes = GetBytes(value);
            var sizeBytes = BitConverter.GetBytes((ushort) valueBytes.Length);

            var sizeOffset = sizeBytes.Length;

            bytes = new byte[sizeOffset + valueBytes.Length];

            for (var i = 0; i < sizeOffset; i++)
                bytes[i] = sizeBytes[i];

            for (var i = 0; i < valueBytes.Length; i++)
                bytes[i + sizeOffset] = valueBytes[i];

            BlockSize = bytes.Length;
        }

        byte[] GetBytes(float[] value)
        {
            var byteArr = new byte[value.Length * ChunkLength];
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < ChunkLength; j++) 
                    byteArr[i * ChunkLength + j] = BitConverter.GetBytes(value[i])[j];
            }

            return byteArr;
        }

        float[] GetValue(byte[] header, int byteArraySize, int startIdx)
        {
            var valueArraySize = byteArraySize/ChunkLength;
            
            var value = new float[valueArraySize];
            for (int i = 0; i < valueArraySize; i++) 
                value[i] = BitConverter.ToSingle(header, startIdx + ChunkLength * i);

            return value;
        }
    }
}