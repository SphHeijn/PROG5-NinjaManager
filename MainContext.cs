using Microsoft.EntityFrameworkCore;

namespace PROG5_NinjaManager
    {
        public class MainContext : DbContext
        {
            public MainContext(DbContextOptions<MainContext> options) : base(options) { }

            // Define DbSets for your tables here
            public DbSet<Ninja> Ninjas { get; set; }
            public DbSet<Equipment> Equipments { get; set; }
            public DbSet<NinjaInventory> NinjaInventories { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                // Call custom methods for creating relationships and seeding data
                CreateRelations(builder);
                SeedData(builder);
            }

            private static void CreateRelations(ModelBuilder builder)
            {
                // Define composite key for NinjaInventory join table
                builder.Entity<NinjaInventory>()
                    .HasKey(ni => new { ni.NinjaId, ni.EquipmentId });

                // Configure many-to-many relationship between Ninja and Equipment
                builder.Entity<NinjaInventory>()
                    .HasOne(ni => ni.Ninja)
                    .WithMany(n => n.NinjaInventories)
                    .HasForeignKey(ni => ni.NinjaId);

                builder.Entity<NinjaInventory>()
                    .HasOne(ni => ni.Equipment)
                    .WithMany(e => e.NinjaInventories)
                    .HasForeignKey(ni => ni.EquipmentId);
            }

            private static void SeedData(ModelBuilder builder)
            {
                // Seed Ninjas
                builder.Entity<Ninja>().HasData(
                    new Ninja { Id = 1, Name = "Shadow", Gold = 100 },
                    new Ninja { Id = 2, Name = "Blade", Gold = 150 }
                );

                // Seed Equipment
                builder.Entity<Equipment>().HasData(
                    new Equipment { Id = 1, Name = "Katana", EquipmentType = EquipmentType.Hand, MonetaryValue = 100, Strength = 15, Intelligence = 5, Agility = 10 },
                    new Equipment { Id = 2, Name = "Shuriken", EquipmentType = EquipmentType.Hand, MonetaryValue = 50, Strength = 5, Intelligence = 0, Agility = 15 },
                    new Equipment { Id = 3, Name = "Ninja Armor", EquipmentType = EquipmentType.Chest, MonetaryValue = 200, Strength = 10, Intelligence = 0, Agility = 5 }
                );

                // Seed NinjaInventory (linking table)
                builder.Entity<NinjaInventory>().HasData(
                    new NinjaInventory { NinjaId = 1, EquipmentId = 1, NinjaName = "Shadow", EquipmentName = "Katana" },
                    new NinjaInventory { NinjaId = 1, EquipmentId = 2, NinjaName = "Shadow", EquipmentName = "Shuriken" },
                    new NinjaInventory { NinjaId = 2, EquipmentId = 3, NinjaName = "Blade", EquipmentName = "Ninja Armor" }
                );
            }
        }

        

        public class Ninja
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Gold { get; set; }
        
            public ICollection<NinjaInventory> NinjaInventories { get; set; } = new List<NinjaInventory>();
        }

        public class NinjaInventory
        {
            public int NinjaId { get; set; }
            public Ninja Ninja { get; set; }

            public int EquipmentId { get; set; }
            public Equipment Equipment { get; set; }

            public string NinjaName { get; set; }
            public string EquipmentName { get; set; }
        }

        public enum EquipmentType
        {
            Head = 1,
            Hand = 1,
            Foot = 1,
            Necklace = 1,
            Chest = 1,
            Ring = 1
        }

        public class Equipment
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public EquipmentType EquipmentType { get; set; }

            public int MonetaryValue { get; set; }
            public int Strength { get; set; }
            public int Intelligence { get; set; }
            public int Agility { get; set; }
        
            public ICollection<NinjaInventory> NinjaInventories { get; set; } = new List<NinjaInventory>();
        }
    }
