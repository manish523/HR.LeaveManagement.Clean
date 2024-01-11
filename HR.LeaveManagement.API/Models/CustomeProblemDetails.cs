using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Models
{
    public class CustomeProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
