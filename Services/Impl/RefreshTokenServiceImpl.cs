using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Request;
using Demo.DTOs.Response;

namespace Demo.Services.Impl; 

public class RefreshTokenServiceImpl : RefreshTokenService {

    private readonly DataContext dataContext;

    public RefreshTokenServiceImpl(DataContext dataContext) {
        this.dataContext = dataContext;
    }

    public void initRefreshToken(Student student) {
        RefreshToken refreshToken = new RefreshToken();
        refreshToken.Token = "";
        refreshToken.Student = student;

        dataContext.RefreshTokens.Add(refreshToken);
        dataContext.SaveChanges();
    }
}