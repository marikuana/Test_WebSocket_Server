public class WebSocketMiddleware
{
    private RequestDelegate _next;
    private WebSocketSessionFactory _sessionFactory;

    public WebSocketMiddleware(RequestDelegate next, WebSocketSessionFactory sessionFactory)
    {
        _next = next;
        _sessionFactory = sessionFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var session = _sessionFactory.GetSession(webSocket);
            context.Items.TryAdd("WebSocketSession", session);
        }
        await _next(context);
    }
}
