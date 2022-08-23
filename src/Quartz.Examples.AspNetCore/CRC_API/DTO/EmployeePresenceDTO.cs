namespace Quartz.Examples.AspNetCore.CRC_API.DTO
{
    public class EmployeePresenceDTO
    {
        public int ResourceID { get; set; }

        public int SiteID { get; set; }

        public int DeviceID { get; set; }

        public DateTime? LastRegistrationDateTime { get; set; }
    }
}
