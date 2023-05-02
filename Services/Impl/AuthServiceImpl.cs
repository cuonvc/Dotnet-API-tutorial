using Demo.Configuration;
using Demo.Converter;
using Demo.DTOs.Request;
using Demo.DTOs.Response;

namespace Demo.Services.Impl; 

public class AuthServiceImpl : AuthService {

    private readonly DataContext context;
    private readonly SecurityConfiguration securityConfiguration;
    private readonly StudentConverter studentConverter;

    public AuthServiceImpl(DataContext context, SecurityConfiguration securityConfiguration,
            StudentConverter studentConverter) {
        this.context = context;
        this.securityConfiguration = securityConfiguration;
        this.studentConverter = studentConverter;
    }

    public ResponseObject<string> regAccount(RegisterRequest request) {
        Student student = studentConverter.regRequestToEntity(request);
        Major major = context.Majors.Find(request.MajorId);
        
        ResponseObject<string> responseObject = new ResponseObject<string>();
        if (major == null) {
            return responseObject.responseError("Major not found with id: " + request.MajorId,
                StatusCodes.Status404NotFound.ToString(), null);
        }
        context.Students.Add(student);
        context.SaveChanges();
        
        return responseObject.responseSuccess("Success", "Registed successfully");
    }

}