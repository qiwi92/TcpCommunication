namespace TCPCommunication.Message
{
    class Int32LayoutBlock : LayoutBlock<int>
    {
        public override int BlockSize { get; protected set; } = 4;
        public override int Read(byte[] headerBytes, int startIdx) => MessageUtils.ToInt(headerBytes, startIdx);
        public override void Write(int value) => bytes = MessageUtils.GetBytes(value);
    }
}