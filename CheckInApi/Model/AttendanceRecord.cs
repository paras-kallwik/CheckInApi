﻿using Azure;
using Azure.Data.Tables;

namespace CheckInApi.Model
{
    public class AttendanceRecord : ITableEntity
    {
        // Required by Azure Table Storage
        public string PartitionKey { get; set; } // e.g., email
        public string RowKey { get; set; }        // e.g., 20250715-CheckIn
        public DateTimeOffset? Timestamp { get; set; } = DateTime.Now;
        public ETag ETag { get; set; }

        // Your custom fields
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        public DateTime Time { get; set; } = DateTime.Now;
        public string Status { get; set; }  // CheckIn / CheckOut
    }
}
