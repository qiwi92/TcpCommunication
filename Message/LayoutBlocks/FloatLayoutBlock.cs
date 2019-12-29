namespace TCPCommunication.Message
{
    class FloatLayoutBlock : LayoutBlock<float>
    {
        public override int BlockSize { get; protected set; } = 4;
        public override float Read(byte[] headerBytes, int startIdx) => MessageUtils.ToFloat(headerBytes, startIdx);
        public override void Write(float value) => bytes = MessageUtils.GetBytes(value);
    }
}