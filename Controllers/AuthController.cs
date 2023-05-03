using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[ApiController]
[Route("/api/auth/")]
public class AuthController : Controller {

    private readonly AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    [HttpPost]
    [Route("signup")]
    public IActionResult registerAccount(RegisterRequest request) {
        ResponseObject<string> response = authService.regAccount(request);
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult loginAccount(LoginRequest request) {
        ResponseObject<string> responseObject = authService.login(request);
        if (responseObject.Data == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }
}