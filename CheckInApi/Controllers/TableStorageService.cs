using Azure.Data.Tables;
using CheckInApi.Model;

namespace CheckInApi.Controllers
{
    public class TableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService(IConfiguration config)
        {
            string connectionString = config["AzureTableStorage:ConnectionString"];
            string tableName = "checkindata";

            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task AddAttendanceAsync(string email, string status)
        {
            var today = DateTime.Now.Date;
            var now = DateTime.Now.TimeOfDay;

            var entity = new AttendanceRecord
            {
                PartitionKey = email.ToLower(),
                RowKey = $"{today:yyyyMMdd}-{status}",
                Date = today,
                Time = DateTime.Now,
                Status = status,
            };

            await _tableClient.AddEntityAsync(entity);
        }
    }

}
