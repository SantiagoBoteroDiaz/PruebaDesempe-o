using GestionDeEspacios.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeEspacios.Data;

public class MysqlDbContext : DbContext
{
    public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User>  Users { get; set; }
    public DbSet<SportSpace>  SportSpaces { get; set; }
    public DbSet<Reservation>  Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enum 
        modelBuilder.Entity<Reservation>()
            .Property(x => x.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<SportSpace>()
            .Property(x => x.Type)
            .HasConversion<string>();

        // Relacions
        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.User)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.UserID)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Reservation>()
            .HasOne(x => x.SportSpace)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.SportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}