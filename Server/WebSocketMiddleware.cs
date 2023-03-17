public class WebSocketMiddleware
{
    private RequestDelegate _next;
    private WebSocketSessionFactory _sessionFactory;
    private ILogger<WebSocketMiddleware> _logger;

    public WebSocketMiddleware(RequestDelegate next, WebSocketSessionFactory sessionFactory, ILogger<WebSocketMiddleware> logger)
    {
        _next = next;
        _sessionFactory = sessionFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var session = _sessionFactory.GetSession(webSocket);
            await session.RecieveAsync();
        }
        else
            await _next(context);
    }
}
