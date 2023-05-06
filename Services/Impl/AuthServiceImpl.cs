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

    public AuthServiceImpl(DataContext context, SecurityConfiguration securityConfiguration,
            StudentConverter studentConverter, IConfiguration configuration) {
        this.context = context;
        this.securityConfiguration = securityConfiguration;
        this.studentConverter = studentConverter;
        this.configuration = configuration;
    }

    public AuthServiceImpl() {
        
    }

    public ResponseObject<string> regAccount(RegisterRequest request) {
        Student student = studentConverter.RegRequestToEntity(request);
        Major major = context.Majors.Find(request.MajorId);
        
        ResponseObject<string> responseObject = new ResponseObject<string>();
        if (major == null) {
            return responseObject.responseError("Major not found with id: " + request.MajorId,
                StatusCodes.Status404NotFound.ToString(), null);
        }

        student.Major = major;
        context.Students.Add(student);
        context.SaveChanges();
        
        return responseObject.responseSuccess("Success", "Registed successfully");
    }

    public ResponseObject<string> login(LoginRequest request) {
        ResponseObject<string> responseObject = new ResponseObject<string>();

        Student accountverified = verifyAccount(request);
        if (accountverified != null) {
            string token = generateToken(accountverified);

            return responseObject.responseSuccess("Success", token);
        }

        return responseObject.responseError("Username or password incorrect", StatusCodes.Status401Unauthorized.ToString(), null);
    }

    private string generateToken(Student student) {
        List<Claim> claims = new List<Claim> {
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

}