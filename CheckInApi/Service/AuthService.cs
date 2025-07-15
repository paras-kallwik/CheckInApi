using CheckInApi.AuthRepository;
using CheckInApi.Model;

namespace CheckInApi.Service
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository=authRepository;
        }
        public bool RegistrationUser(UserData userData)
        {
            if (userData==null)
            {
                Console.WriteLine("Userdata does not Contain");
            }
          return _authRepository.Registration(userData);
        }
        public UserData Login(LoginDto loginDto)
        {
            if (loginDto==null)
            {
                Console.WriteLine("logindto does not Contain");
            }
            return _authRepository.Login(loginDto);
        }


    }
}
