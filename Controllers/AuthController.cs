using Demo.DTOs.Request;
using Demo.DTOs.Response;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[ApiController]
[Route("/api/auth/")]
public class AuthController {

    private readonly AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    [HttpPost]
    [Route("signup")]
    public ResponseObject<string> registerAccount(RegisterRequest request) {
        ResponseObject<string> response = authService.regAccount(request);
        return response;
    }
}