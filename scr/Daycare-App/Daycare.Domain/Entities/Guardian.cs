using Daycare.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daycare.Domain.Entities
{
    public class Guardian : Person
    {
        public string Relationship { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public bool IsEmergencyContact { get; set; } = true;

        public ICollection<Child>? Children { get; set; }
    }
}

