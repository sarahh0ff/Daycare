using Daycare.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daycare.Domain.Entities
{
    public class Child : Person
    {
        public string EnrollmentNumber { get; set; } = string.Empty; 
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;  
        public string? Allergies { get; set; } 
        public string? Notes { get; set; } 

        public int GuardianId { get; set; }
    }
}
