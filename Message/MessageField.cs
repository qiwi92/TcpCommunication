using System;

namespace TCPCommunication.Message
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MessageField : Attribute
    {
        public readonly int Order;
        public MessageField(int order) => Order = order;
    }
}