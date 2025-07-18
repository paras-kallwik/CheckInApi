using Azure;
using Azure.Data.Tables;
using System;

namespace CheckInApi.Model
{
    public class UserData : ITableEntity
    {
        public string PartitionKey { get; set; } = "users";  // Common partition
        public string RowKey { get; set; }                   // We'll use email as RowKey
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom properties
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }  // duplicate of RowKey, but convenient
        public string Phone { get; set; }
        public string Password { get; set; }

        // Optional: Full Name
        public string Name => $"{Fname} {Lname}";
    }
}
