using CheckInApi.Model;

namespace CheckInApi.Service
{
    public interface IAuthService
    {
        public bool RegistrationUser(UserData userData);
        public UserData Login(LoginDto loginDto);

    }
}
