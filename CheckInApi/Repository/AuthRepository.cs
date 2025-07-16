using CheckInApi.AuthRepository;
using CheckInApi.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using System.Diagnostics.Eventing.Reader;

namespace CheckInApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserDbContext _userDbContext;
        public AuthRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public bool  Registration(UserData userData)
        {
            if (_userDbContext.Usersdata.Any(x=>x.Email==userData.Email))
            {
               return false;
            }
            else
            {
                _userDbContext.Usersdata.Add(userData);
                _userDbContext.SaveChanges();
                return true;
            }
        }

        public UserData Login(LoginDto loginDto)
        {
            return _userDbContext.Usersdata
                .FirstOrDefault(x => x.Email == loginDto.Email && x.Password == loginDto.Password);
        }
    }
}
