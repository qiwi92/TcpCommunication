namespace TCPCommunication.Message
{
    public interface IHeaderBytes<out T>
    {
        byte[] GetBytes();
        T ToValue(byte[] bytes, int startIdx);
    }
}
    public interface ITcpMessage<out T>
    {
        T FromBytes(byte[] bytes);
        byte[] GetBytes();
    }

    public abstract class TcpMessage
    {
        
    }
    
    
