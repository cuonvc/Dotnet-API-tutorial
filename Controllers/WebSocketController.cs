using System.Net.WebSockets;
using System.Text;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers; 

[ApiController]
public class WebSocketController : ControllerBase {

    private readonly WebSocketService webSocketService;
    private readonly ILogger<WebSocketController> logger;

    public WebSocketController(WebSocketService webSocketService, ILogger<WebSocketController> logger) {
        this.webSocketService = webSocketService;
        this.logger = logger;
    }

    [HttpGet("/ws")]
    public async Task Get() {
        if (HttpContext.WebSockets.IsWebSocketRequest) {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            logger.Log(LogLevel.Information, "WebSocket connection established");
            await webSocketService.echo(webSocket);
        } else {
            HttpContext.Response.StatusCode = 400;
        }
    }
}