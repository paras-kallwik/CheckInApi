using Azure.Data.Tables;
using CheckInApi.Model;

namespace CheckInApi.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly TableClient _attendanceTable;
        private readonly TableClient _userTable;

        public AttendanceService(IConfiguration config)
        {
            string connectionString = config["AzureTableStorage:ConnectionString"];

            _attendanceTable = new TableClient(connectionString, "AttendanceRecords");
            _userTable = new TableClient(connectionString, "UserData");

            _attendanceTable.CreateIfNotExists();
            _userTable.CreateIfNotExists();
        }

        public async Task<string> MarkAttendanceAsync(string email, string status)
        {
            var userResponse = await _userTable.GetEntityIfExistsAsync<UserData>("users", email.ToLower());
            if (!userResponse.HasValue)
                return "User not found!";

            var user = userResponse.Value;
            var today = DateTime.UtcNow.Date;
            string rowKey = $"{today:yyyyMMdd}-{status}";

            if (status == "CheckIn")
            {
                var check = await _attendanceTable.GetEntityIfExistsAsync<AttendanceRecord>(email.ToLower(), rowKey);
                if (check.HasValue)
                    return "Already checked in for today.";
            }
            else if (status == "CheckOut")
            {
                var checkInKey = $"{today:yyyyMMdd}-CheckIn";
                var checkIn = await _attendanceTable.GetEntityIfExistsAsync<AttendanceRecord>(email.ToLower(), checkInKey);

                if (!checkIn.HasValue)
                    return "You haven't checked in today.";

                var checkOut = await _attendanceTable.GetEntityIfExistsAsync<AttendanceRecord>(email.ToLower(), rowKey);
                if (checkOut.HasValue)
                    return "You have already checked out for today.";
            }

            var record = new AttendanceRecord
            {
                PartitionKey = email.ToLower(),
                RowKey = rowKey,
                Email = email,
                Date = today,
                Time = DateTime.UtcNow,
                Status = status
            };

            await _attendanceTable.AddEntityAsync(record);
            return $"Successfully marked {status} at {record.Time.ToLocalTime():HH:mm:ss}";

        }
    }
}
