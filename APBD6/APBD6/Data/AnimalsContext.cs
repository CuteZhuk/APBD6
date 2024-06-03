namespace APBD6.Data;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class AnimalsContext : DbContext
{
    public AnimalsContext(DbContextOptions<AnimalsContext> options) : base(options) { }

    public DbSet<Animal> Animals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasKey(a => a.IdAnimal);

        base.OnModelCreating(modelBuilder);
    }
}

public class Animal
{
    [Key]
    public int IdAnimal { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}