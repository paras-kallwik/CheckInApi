using CheckInApi.Model;

namespace CheckInApi.Service
{
    public interface IAuthService
    {
        Task<bool> RegistrationUserAsync(UserData userData);
        Task<UserData?> LoginUserAsync(LoginDto loginDto);
    }
}
