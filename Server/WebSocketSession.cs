using System.Net.WebSockets;

public class WebSocketSession
{
    private WebSocket _webSocket;
    private ILogger _logger;

    public WebSocketSession(WebSocket webSocket, ILogger<WebSocketSession> logger)
    {
        _webSocket = webSocket;
        _logger = logger;
    }

    public async Task RecieveAsync()
    {
        byte[] buf = new byte[1024];

        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(buf, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
                break;

            byte[] receiveBuf = new byte[result.Count];
            Array.Copy(buf, receiveBuf, receiveBuf.Length);

            await _webSocket.SendAsync(receiveBuf, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
    }
}
