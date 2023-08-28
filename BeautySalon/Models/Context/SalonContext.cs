using BeautySalon.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

namespace BeautySalon.Models.Context
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<SubJob>().HasQueryFilter(w=>w.Name != "صاحب آرایشگاه");

            base.OnModelCreating(modelBuilder);

        }

        #region Context

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<SubJob> SubJob { get; set; }
        public DbSet<WorkingDays> WorkingDays { get; set; }
        public DbSet<WorkingTime> WorkingTime { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<Weblogs> Weblogs { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Specifications> Specifications { get; set; }

        #endregion

    }

    public class ToDoContextFactory : IDesignTimeDbContextFactory<SalonContext>
    {
        public SalonContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SalonContext>();
            builder.UseSqlServer("Data Source=.;Initial Catalog=BeautySalon;Integrated Security=True;MultipleActiveResultSets=true");
            return new SalonContext(builder.Options);
        }
    }
}
