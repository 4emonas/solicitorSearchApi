using Microsoft.EntityFrameworkCore;
using SolicitorSearch.DataAccess.Contexts.Abstract;
using SolicitorSearch.DataAccess.Entities;

namespace SolicitorSearch.DataAccess.Contexts
{
    public class PostgresContext : DbContext, IPostgresContext
    {
        public PostgresContext(DbContextOptions<PostgresContext> options)
            : base(options)
        {
        }

        public DbSet<SolicitorEntity> Solicitors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SolicitorEntity>(eb =>
            {
                eb.ToTable("solicitor");
                eb.HasKey(e => e.Id).HasName("pk_solicitor_id");
                eb.Property(e => e.Id).HasColumnName("id");
                eb.Property(e => e.Name).HasColumnName("name");
                eb.Property(e => e.Address).HasColumnName("address");
                eb.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
                eb.Property(e => e.Notes).HasColumnName("notes");
                eb.Property(e => e.City).HasColumnName("city");
            });
        }
    }
}
