using CheckInApi.Model;

namespace CheckInApi.Service
{
    public interface IAuthService
    {
        public bool RegistrationUser(UserData userData);
        public UserData LoginUser(LoginDto loginDto);

    }
}
