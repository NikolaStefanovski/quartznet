using Microsoft.Extensions.Options;

namespace Quartz.Examples.AspNetCore.CRC_API
{
    public class CRCOptions
    {
        public string? BaseURI { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
