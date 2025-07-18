using Azure.Data.Tables;
using CheckInApi.Model;

namespace CheckInApi.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TableClient _userTable;

        public AuthRepository(TableServiceClient tableServiceClient)
        {
            _userTable = tableServiceClient.GetTableClient("UserData");
            _userTable.CreateIfNotExists();
        }

        public async Task<bool> RegistrationAsync(UserData userData)
        {
            // Set PartitionKey and RowKey properly
            userData.PartitionKey = "users";
            userData.RowKey = userData.Email.ToLower().Trim();

            // Check if user already exists
            await foreach (var existingUser in _userTable.QueryAsync<UserData>(
                u => u.PartitionKey == userData.PartitionKey && u.RowKey == userData.RowKey))
            {
                return false; // User already exists
            }

            // Insert new user
            await _userTable.AddEntityAsync(userData);
            return true;
        }



        public async Task<UserData?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                string partitionKey = "users";
                string rowKey = loginDto.Email.ToLower().Trim();

                var response = await _userTable.GetEntityAsync<UserData>(partitionKey, rowKey);

                if (response.Value.Password == loginDto.Password)
                {
                    return response.Value;
                }

                return null; // Password doesn't match
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                // User not found
                Console.WriteLine("Login failed: User not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return null;
            }
        }



    }
}
