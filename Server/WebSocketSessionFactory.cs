using System.Net.WebSockets;

public class WebSocketSessionFactory
{
    private ILoggerFactory _loggerFactory;

    public WebSocketSessionFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public WebSocketSession GetSession(WebSocket webSocket)
        => new WebSocketSession(webSocket, _loggerFactory.CreateLogger<WebSocketSession>());
}