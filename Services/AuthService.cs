using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;

namespace Demo.Services; 

public interface AuthService {

    ResponseObject<Student> regAccount(RegisterRequest request);

    ResponseObject<RefreshTokenResponse> login(LoginRequest request);

    ResponseObject<RefreshTokenResponse> renewAccess(RefreshTokenRequest request);

    void logout();
}