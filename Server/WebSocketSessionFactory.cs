using System.Net.WebSockets;

public class WebSocketSessionFactory
{
    public WebSocketSession GetSession(WebSocket webSocket)
        => new WebSocketSession(webSocket);
}