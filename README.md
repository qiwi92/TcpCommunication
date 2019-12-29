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
Simple Unity Example
=====
Server (Unity Project A):
```csharp
using TCPCommunication;
using TCPCommunication.Message;
using UnityEngine;

public class TcpServerView : MonoBehaviour
{
    TcpServer _server;

    void Awake()
    {
        _server = new TcpServer();
        _server.Initialize();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var message = new TestMessage()
            {
                SomeFloatValue = Random.Range(.1f, .3f),
                AnotherIntValue = Random.Range(10, 300),
            };
            Debug.Log("Server send: " + message);
            _server.SendMessage(MessageBuilder.GetBytes(message));
        }
    }

    void OnDestroy() => _server.Stop();
}
```

Client (Unity Project B):
```csharp
using TCPCommunication;
using TCPCommunication.Message;
using UnityEngine;

public class TcpClientView : MonoBehaviour
{
    TcpClient _client;

    void Awake()
    {
        _client = new TcpClient();
        _client.Initialize();
        _client.OnBytesReceived += BytesReceived;
    }

    void BytesReceived(byte[] bytes)
    {
        var message = MessageBuilder.GetMessage<TestMessage>(bytes);
        Debug.Log("Client Received: " + message);
    }

    void OnDestroy() => _client.Stop();
}
```
