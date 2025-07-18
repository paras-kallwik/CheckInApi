using CheckInApi.AuthRepository;
using CheckInApi.Model;

namespace CheckInApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<bool> RegistrationUserAsync(UserData userData)
        {
            return await _authRepository.RegistrationAsync(userData);
        }

        public async Task<UserData?> LoginUserAsync(LoginDto loginDto)
        {
            return await _authRepository.LoginAsync(loginDto);
        }
    }
}
