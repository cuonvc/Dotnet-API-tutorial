using System.Net.WebSockets;

namespace Demo.Services; 

public interface WebSocketService {
    public Task echo(WebSocket webSocket);
}