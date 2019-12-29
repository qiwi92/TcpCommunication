namespace TCPCommunication.Message
{
    public class StringLayoutBlock : LayoutBlock<string>
    {
        public override int BlockSize { get; protected set; }

        public override string Read(byte[] headerBytes, int startIdx)
        {
            var value = MessageUtils.ToString(headerBytes, startIdx, out var length);
            BlockSize = length + 2;
            return value;
        }

        public override void Write(string value)
        {
            bytes = MessageUtils.GetBytes(value);
            BlockSize = bytes.Length;
        }
    }
}