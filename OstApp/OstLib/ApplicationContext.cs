using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OstLib
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<TimeTableEntry> TimeTableEntry { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.Migrate();
            Context.AddDb(this);
        }

        public static DbContextOptions<ApplicationContext> GetDb()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            return optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=osteopat;Username=postgres;Password=denchik2702").UseLazyLoadingProxies().Options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().Ignore(s => s.GetName);
            modelBuilder.Entity<Client>().Ignore(s => s.GetNameWithoutMiddleName);
            modelBuilder.Entity<Client>().Ignore(s => s.GetNameWithPhone);
            modelBuilder.Entity<Appointment>().Ignore(s => s.GetDate);
            modelBuilder.Entity<TimeTableEntry>().Ignore(s => s.GetDate);
            modelBuilder.Entity<TimeTableEntry>().Ignore(s => s.GetTime);
        }
    }
}
