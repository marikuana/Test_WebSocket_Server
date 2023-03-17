using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Xunit;
using System;
using System.Threading;
using System.Net.WebSockets;
using MsgPack.Serialization;
using System.IO;

namespace ServerTests
{
    public class ServerTests
    {
        private readonly TestServer _server;

        public ServerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<WebSocketSessionFactory>();
                })
                .Configure(configure =>
                {
                    configure.UseMiddleware<WebSocketMiddleware>();
                }));
        }

        [Fact]
        public async void TestConnect()
        {
            var client = _server.CreateWebSocketClient();
            var ws = await client.ConnectAsync(new Uri("ws://local"), CancellationToken.None);

            Assert.NotNull(ws);
            Assert.Equal(WebSocketState.Open, ws.State);
        }

        [Fact]
        public async void TestSendReceiveBytes()
        {
            var client = _server.CreateWebSocketClient();
            var ws = await client.ConnectAsync(new Uri("ws://local"), CancellationToken.None);

            byte[] sendBuf = new byte[] { 1, 2, 3 };
            await ws.SendAsync(sendBuf, WebSocketMessageType.Binary, true, CancellationToken.None);

            byte[] receiveBuf = new byte[1024];
            var result = await ws.ReceiveAsync(receiveBuf, CancellationToken.None);

            byte[] receiveMessage = new byte[result.Count];
            Array.Copy(receiveBuf, receiveMessage, result.Count);

            Assert.Equal(sendBuf.Length, result.Count);
            Assert.Equal(sendBuf, receiveMessage);
        }

        [Fact]
        public async void TestSendReceiveTextMessageModel()
        {
            var client = _server.CreateWebSocketClient();
            var ws = await client.ConnectAsync(new Uri("ws://local"), CancellationToken.None);
            var serializer = MessagePackSerializer.Get<TextMessageModel>();
            var textMessageSend = new TextMessageModel() { Text = "Test Message" };

            var streamSend = new MemoryStream();
            serializer.Pack(streamSend, textMessageSend);
            byte[] sendBuf = streamSend.ToArray();
            await ws.SendAsync(sendBuf, WebSocketMessageType.Binary, true, CancellationToken.None);

            byte[] receiveBuf = new byte[1024];
            var result = await ws.ReceiveAsync(receiveBuf, CancellationToken.None);
            using var streamReceive = new MemoryStream(receiveBuf, 0, result.Count);
            var textMessageReceive = serializer.Unpack(streamReceive);
            
            Assert.Equal(textMessageSend.Text, textMessageReceive.Text);
            Assert.NotEqual(textMessageSend, textMessageReceive);
        }
    }
}