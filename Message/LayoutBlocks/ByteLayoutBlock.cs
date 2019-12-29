namespace TCPCommunication.Message
{
    class ByteLayoutBlock : LayoutBlock<byte>
    {
        public override int BlockSize { get; protected set; } = 1;
        public override byte Read(byte[] headerBytes, int startIdx) => headerBytes[startIdx];
        public override void Write(byte value) => bytes = new[] {value};
    }
}