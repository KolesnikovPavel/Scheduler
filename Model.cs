using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Scheduler
{
    public class InspectionContext : DbContext
    {
        public DbSet<Inspector> Inspectors { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\Pavel\source\repos\Scheduler\Inspection.db");
    }

    public class Inspector
    {
        public int InspectorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Preferences { get; set; }
        public string Status { get; set; }
        public string VacationStart { get; set; }
        public string VacationEnd { get; set; }

    }
    public class Inspection
    {
        public int InspectionId { get; set; }
        public string Classifier { get; set; }
        public string Address { get; set; }
        public string Date { get; set; }
        public int Time { get; set; }
        public string ActingInspector { get; set; }
    }
}