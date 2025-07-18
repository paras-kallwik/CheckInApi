using CheckInApi.Model;

namespace CheckInApi.AuthRepository
{
    public interface IAuthRepository
    {
        Task<bool> RegistrationAsync(UserData userData);
        Task<UserData?> LoginAsync(LoginDto loginDto);
    }
}
