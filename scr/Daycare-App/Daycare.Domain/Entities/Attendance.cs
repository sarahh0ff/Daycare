using Daycare.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daycare.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public int ChildId { get; set; }
        public Child? Child { get; set; }

        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public bool IsPresent { get; set; } = true;
    }


}
