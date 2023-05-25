using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;

namespace Demo.Services; 

public interface RefreshTokenService {

    void initRefreshToken(Student student);
}