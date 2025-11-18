using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daycare.Application.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }

        public int ChildId { get; set; }

        public int ActivityId { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}

