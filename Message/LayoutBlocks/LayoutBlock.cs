namespace TCPCommunication.Message
{
    public abstract class LayoutBlock
    {
        public abstract byte[] Bytes { get; }
    }
    
    public abstract class LayoutBlock<T> : LayoutBlock
    {
        public abstract int BlockSize { get; protected set; }
        public override byte[] Bytes => bytes;
        protected byte[] bytes;

        public abstract T Read(byte[] headerBytes, int startIdx);
        public abstract void Write(T value);
    }
}