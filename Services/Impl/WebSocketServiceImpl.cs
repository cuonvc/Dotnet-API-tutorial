using System.Net.WebSockets;
using System.Text;

namespace Demo.Services.Impl; 

public class WebSocketServiceImpl : WebSocketService {

    private readonly ILogger<WebSocketServiceImpl> logger;

    public WebSocketServiceImpl(ILogger<WebSocketServiceImpl> logger) {
        this.logger = logger;
    }

    public async Task echo(WebSocket webSocket) {
        var buffer = new byte[1024 * 4];  //convert bits to bytes
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue) {
            var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. Server said: {Encoding.UTF8.GetString(buffer)}");
            await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);

            buffer = new byte[1024 * 4];
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //after received message, if result is close status (close connection) -> break out of this while loop
            
        }
        
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        logger.Log(LogLevel.Information, "WebSocket connection closed");
    }
    
}