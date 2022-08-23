using System.Net;

namespace Quartz.Examples.AspNetCore.CRC_API.Models
{
    public class Result<T>
    {
        public string? Error { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public T? Value { get; set; }
    }
}
