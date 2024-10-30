using Microsoft.EntityFrameworkCore;

namespace PROG5_NinjaManager
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }

        // Define DbSets for your tables here
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Weapon> Weapons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Call custom methods for creating relationships and seeding data
            CreateRelations(builder);
            SeedData(builder);
        }

        private static void CreateRelations(ModelBuilder builder)
        {
            // Define relationships between entities, if any.
            // For example, if a Ninja can have multiple Weapons:
            builder.Entity<Ninja>()
                .HasMany(n => n.Weapons)
                .WithOne(w => w.Ninja)
                .HasForeignKey(w => w.NinjaId);
        }

        private static void SeedData(ModelBuilder builder)
        {
            // Seed data to Ninjas table
            builder.Entity<Ninja>().HasData(
                new Ninja { Id = 1, Name = "Shadow", SkillLevel = 5 },
                new Ninja { Id = 2, Name = "Blade", SkillLevel = 7 }
            );

            // Seed data to Weapons table
            builder.Entity<Weapon>().HasData(
                new Weapon { Id = 1, Name = "Katana", Damage = 10, NinjaId = 1 },
                new Weapon { Id = 2, Name = "Shuriken", Damage = 5, NinjaId = 2 }
            );
        }
    }

    public class Ninja
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SkillLevel { get; set; }
        public List<Weapon> Weapons { get; set; }
    }

    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int NinjaId { get; set; }
        public Ninja Ninja { get; set; }
    }
}
