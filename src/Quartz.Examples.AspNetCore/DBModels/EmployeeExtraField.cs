using System.ComponentModel.DataAnnotations.Schema;

namespace Quartz.Examples.AspNetCore.DBModels
{
    [Table("EmployeeExtraField")]
    public class EmployeeExtraField
    {
        public string? Code { get; set; }
        public string? Value { get; set; }

        // relation to Employee
        public int ID { get; set; }
        public Employee? Employee { get; set; }

    }
}
