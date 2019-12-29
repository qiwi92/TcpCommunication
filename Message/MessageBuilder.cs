using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TCPCommunication.Message
{
    public static class MessageBuilder
    {
        public static byte[] GetBytes(Message message)
        {
            var headerFields = GetHeaderFields(message);

            var layoutBlocks = new List<LayoutBlock>();
            foreach (var fieldInfo in headerFields)
            {
                var fieldType = fieldInfo.FieldType;

                if (fieldType == typeof(byte))
                    layoutBlocks.Add(Write<ByteLayoutBlock, byte>(message, fieldInfo));

                if (fieldType == typeof(int))
                    layoutBlocks.Add(Write<Int32LayoutBlock, int>(message, fieldInfo));

                if (fieldType == typeof(ushort))
                    layoutBlocks.Add(Write<UshortLayoutBlock, ushort>(message, fieldInfo));

                if (fieldType == typeof(float))
                    layoutBlocks.Add(Write<FloatLayoutBlock, float>(message, fieldInfo));

                if (fieldType == typeof(string))
                    layoutBlocks.Add(Write<StringLayoutBlock, string>(message, fieldInfo));

                if (fieldType == typeof(float[]))
                    layoutBlocks.Add(Write<FloatArrayLayoutBlock, float[]>(message, fieldInfo));

                if (fieldType == typeof(DateTime))
                    layoutBlocks.Add(Write<DateTimeLayoutBlock, DateTime>(message, fieldInfo));
            }

            var byteList = new List<byte>();

            foreach (var layoutBlock in layoutBlocks)
                byteList.AddRange(layoutBlock.Bytes);

            return byteList.ToArray();
        }

        public static T GetMessage<T>(byte[] bytes) where T : Message, new()
        {
            var message = new T();
            var messageFields = GetHeaderFields(message);

            var idx = 0;
            foreach (var fieldInfo in messageFields)
            {
                var fieldType = fieldInfo.FieldType;

                if (fieldType == typeof(byte))
                    Read<ByteLayoutBlock, byte>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(int))
                    Read<Int32LayoutBlock, int>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(ushort))
                    Read<UshortLayoutBlock, ushort>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(float))
                    Read<FloatLayoutBlock, float>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(string))
                    Read<StringLayoutBlock, string>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(float[]))
                    Read<FloatArrayLayoutBlock, float[]>(ref idx, bytes, message, fieldInfo);

                if (fieldType == typeof(DateTime))
                    Read<DateTimeLayoutBlock, DateTime>(ref idx, bytes, message, fieldInfo);
            }

            return message;
        }


        static IOrderedEnumerable<FieldInfo> GetHeaderFields(Message message)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            var fieldValuesWithAttribute = message
                .GetType()
                .GetFields(bindingFlags)
                .Where(field =>
                    field.GetCustomAttributes(typeof(MessageField), false).FirstOrDefault() is MessageField)
                .OrderBy(field =>
                    ((MessageField) field.GetCustomAttributes(typeof(MessageField), false).First()).Order);
            return fieldValuesWithAttribute;
        }


        static void Read<T, TV>(ref int startIdx, byte[] bytes, Message message, FieldInfo fieldInfo)
            where T : LayoutBlock<TV>, new()
        {
            var block = new T();
            var value = block.Read(bytes, startIdx);
            fieldInfo.SetValue(message, value);
            startIdx += block.BlockSize;
        }

        static T Write<T, TV>(Message message, FieldInfo fieldInfo) where T : LayoutBlock<TV>, new()
        {
            var block = new T();
            var value = (TV) fieldInfo.GetValue(message);
            block.Write(value);
            return block;
        }
    }
}