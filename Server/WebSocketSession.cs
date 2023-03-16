using System.Net.WebSockets;

public class WebSocketSession
{
    private WebSocket _webSocket;

    public WebSocketSession(WebSocket webSocket)
    {
        _webSocket = webSocket;
        Recieve();
    }

    private async void Recieve()
    {
        while (true)
        {
            if (_webSocket.CloseStatus != null)
                break;

            byte[] buf = new byte[1024];

            await _webSocket.ReceiveAsync(buf, CancellationToken.None);

            await _webSocket.SendAsync(buf, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
    }
}
