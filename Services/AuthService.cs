using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;

namespace Demo.Services; 

public interface AuthService {

    ResponseObject<string> regAccount(RegisterRequest request);

    ResponseObject<string> login(LoginRequest request);
}