namespace Quartz.Examples.AspNetCore.CRC_API.Models
{
    public class EmployeeDTO
    {
        public int ID { get; set; }

        public string? ExternalID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FirmName { get; set; }

        public string? Language { get; set; }

        public string? CurrentDepartmentID { get; set; }

        public IEnumerable<ExtraFieldDTO>? ExtraFields { get; set; }
    }

    public class ExtraFieldDTO
    {
        public string? Code { get; set; }

        public string? Value { get; set; }
    }
}
