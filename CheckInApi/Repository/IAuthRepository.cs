using CheckInApi.Model;

namespace CheckInApi.AuthRepository
{
    public interface IAuthRepository
    {
          public bool  Registration(UserData userData);
          public UserData Login(LoginDto loginDto);
    }
}
