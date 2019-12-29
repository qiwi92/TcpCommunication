# Tcp Communication
Extremly simple local interprocess communcation framework. Handy when sending data from a local server to a local client and viceversa.

Message
======
With `Message` you can easily create messages that can be converted into a byte-array and viceversa. 
The attribute `[MessageField(int order)]`  allows for soring in the resulting byte array, its recommended to speficy the order as not doing so may lead to unpredictable behaviour.

```csharp
public class TestMessage : Message
{
    [MessageField(0)] public float SomeFloatValue;
    [MessageField(1)] public int AnotherIntValue;
}
```

The above message can be easily converted to a byte-array using the `MessageBuilder`:
```csharp
var message = new TestMessage()
{
    SomeFloatValue = 2.5f,
    AnotherIntValue = 467
};

var byteArray = MessageBuilder.GetBytes(message);
```
and converted back:
```csharp
var message = MessageBuilder.GetMessage<TestMessage>(byteArray);
```
