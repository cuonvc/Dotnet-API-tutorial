using Demo.Converter;
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
    private readonly RefreshTokenService refreshTokenService;
    private readonly StudentConverter studentConverter;

    public AuthController(AuthService authService, RefreshTokenService refreshTokenService,
            StudentConverter studentConverter) {
        this.authService = authService;
        this.refreshTokenService = refreshTokenService;
        this.studentConverter = studentConverter;
    }

    [HttpPost]
    [Route("signup")]
    public IActionResult registerAccount(RegisterRequest request) {
        ResponseObject<Student> response = authService.regAccount(request);
        if (response.Data != null) {
            refreshTokenService.initRefreshToken(response.Data);
            return Ok(studentConverter.ToDTO(response.Data));
        }

        return Unauthorized(response.Message);
    }

    [HttpPost]
    [Route("login")]
    public IActionResult loginAccount(LoginRequest request) {
        ResponseObject<RefreshTokenResponse> responseObject = authService.login(request);
        if (responseObject.Data == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPost]
    [Route("token/renew")]
    public IActionResult renewAccessToken(RefreshTokenRequest request) {
        ResponseObject<RefreshTokenResponse> responseObject = authService.renewAccess(request);
        if (responseObject.Data == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPost]
    [Route("logout")]
    public IActionResult logout() {
        authService.logout();
        return Ok("Logged out");
    }
}