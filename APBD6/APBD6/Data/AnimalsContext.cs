namespace APBD6.Data;

using Microsoft.EntityFrameworkCore;

public class AnimalsContext : DbContext
{
    public AnimalsContext(DbContextOptions<AnimalsContext> options) : base(options) { }

    public DbSet<Animal> Animals { get; set; }
}

public class Animal
{
    public int IdAnimal { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Area { get; set; }
}