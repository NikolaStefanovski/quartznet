using Quartz.Examples.AspNetCore.CRC_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quartz.Examples.AspNetCore.DBModels
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public string? ExternalID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FirmName { get; set; }
        public string? Language { get; set; }
        public string? CurrentDepartmentID { get; set; }

        // HAS EmployeeExtraFields
        public IEnumerable<ExtraFieldDTO>? ExtraFields { get; set; }
    }
}
