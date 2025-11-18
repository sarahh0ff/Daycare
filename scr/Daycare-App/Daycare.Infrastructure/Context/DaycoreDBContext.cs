using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Daycare.Domain.Core;
using Daycare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Daycare.Infrastructure.Context
{
    public class DaycareDBContext : DbContext
    {
        public DaycareDBContext(DbContextOptions<DaycareDBContext> options)
            : base(options)
        {
        }

        public DbSet<Child> Children { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

    }

}