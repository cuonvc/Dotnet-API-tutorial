using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo.Configuration;
using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Services.Impl; 

public class AuthServiceImpl : AuthService {

    private readonly DataContext context;
    private readonly SecurityConfiguration securityConfiguration;
    private readonly StudentConverter studentConverter;
    private readonly IConfiguration configuration;
    private readonly RefreshTokenService refreshTokenService;

    private static readonly long refreshTokenExpireOnMs = 604800000L;  //7day

    public AuthServiceImpl(DataContext context, SecurityConfiguration securityConfiguration,
            StudentConverter studentConverter, IConfiguration configuration, RefreshTokenService refreshTokenService) {
        this.context = context;
        this.securityConfiguration = securityConfiguration;
        this.studentConverter = studentConverter;
        this.configuration = configuration;
        this.refreshTokenService = refreshTokenService;
    }

    public AuthServiceImpl() {
        
    }

    public ResponseObject<Student> regAccount(RegisterRequest request) {
        Student student = studentConverter.RegRequestToEntity(request);
        Major major = context.Majors.Find(request.MajorId);
        
        ResponseObject<Student> responseObject = new ResponseObject<Student>();
        if (major == null) {
            return responseObject.responseError("Major not found with id: " + request.MajorId,
                StatusCodes.Status404NotFound.ToString(), null);
        }
        
        student.Major = major;
        context.Students.Add(student);
        context.SaveChanges();
        
        return responseObject.responseSuccess("Success", student);
    }

    public ResponseObject<RefreshTokenResponse> login(LoginRequest request) {
        ResponseObject<RefreshTokenResponse> responseObject = new ResponseObject<RefreshTokenResponse>();

        Student accountverified = verifyAccount(request);
        if (accountverified != null) {
            string accessToken = generateToken(accountverified);
            RefreshToken refreshToken = updateRefreshToken(accountverified.Id);

            if (refreshToken == null) {
                return responseObject.responseError("Unrecognize refresh token", 
                    StatusCodes.Status401Unauthorized.ToString(), null);
            }

            RefreshTokenResponse response = new RefreshTokenResponse();
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken.Token;
            response.ExpireDate = new DateTime(DateTime.Now.Millisecond + refreshTokenExpireOnMs);
            return responseObject.responseSuccess("Success", response);
        }

        return responseObject.responseError("Username or password incorrect", StatusCodes.Status401Unauthorized.ToString(), null);
    }

    public void logout() {
        
    }
    
    private RefreshToken updateRefreshToken(int studentId) {

        RefreshToken refreshToken = context.RefreshTokens
            .FromSql($"SELECT * FROM RefreshTokens WHERE StudentId = {studentId}")
            .FirstOrDefault();

        if (refreshToken == null) {
            return null;
        }

        refreshToken.Token = "refresh_" + Guid.NewGuid().ToString().Replace("-", "");
        refreshToken.ExpireDate = DateTime.Now.AddDays(7);
        refreshToken.ModifiedDate = DateTime.Now;

        context.SaveChanges();
        return refreshToken;
    }

    private string generateToken(Student student) {
        List<Claim> claims = new List<Claim> {
            new Claim("Id", student.Id.ToString()),
            new Claim("Username", student.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("Jwt:Secret-key").Value!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Student verifyAccount(LoginRequest request) {
        SecurityConfiguration securityConfiguration = new SecurityConfiguration();
        string username = request.Username;
        Student getByUsername = context.Students
            .FromSql($"SELECT * FROM Students WHERE Username = {username}")
            .FirstOrDefault();

        if (getByUsername == null) {
            return null;
        }
        
        string passwordEncoded = securityConfiguration.encodePassword(request.Password, getByUsername.salt);
        if (passwordEncoded != getByUsername.Password) {
            return null;
        }

        return getByUsername;
    }

    public ResponseObject<RefreshTokenResponse> renewAccess(RefreshTokenRequest request) {
        ResponseObject<RefreshTokenResponse> responseObject = new ResponseObject<RefreshTokenResponse>();

        RefreshToken refreshToken = context.RefreshTokens
            .FromSql($"SELECT * FROM RefreshTokens WHERE Token = {request.RefreshToken} AND ExpireDate > {DateTime.Now}")
            .FirstOrDefault();

        if (refreshToken == null) {
            return responseObject.responseError("Unrecognize refresh token", 
                StatusCodes.Status401Unauthorized.ToString(), null);
        }

        RefreshTokenResponse response = new RefreshTokenResponse();
        response.AccessToken = generateToken(refreshToken.Student);
        response.RefreshToken = refreshToken.Token;
        response.ExpireDate = DateTime.Now;

        return responseObject.responseSuccess("Success", response);
    }
}