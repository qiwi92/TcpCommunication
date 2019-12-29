namespace TCPCommunication.Message
{
    public class UshortLayoutBlock : LayoutBlock<ushort>
    {
        public override int BlockSize { get; protected set; } = 2;
        public override ushort Read(byte[] headerBytes, int startIdx) => MessageUtils.ToUshort(headerBytes, startIdx);
        public override void Write(ushort value) => bytes = MessageUtils.GetBytes(value);
    }
}