namespace CheckInApi.Service
{
    public interface IAttendanceService
    {
        Task<string> MarkAttendanceAsync(string email, string status);
    }
}
