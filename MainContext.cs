using Microsoft.EntityFrameworkCore;

namespace PROG5_NinjaManager
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        // Define DbSets for your tables here
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<NinjaInventory> NinjaInventories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Equipment>().HasKey(e => e.Name);
            builder.Entity<Ninja>().HasKey(e => e.Name);
            
            CreateRelations(builder);
            SeedData(builder);
        }

        private static void CreateRelations(ModelBuilder builder)
        {
            // Define composite key using NinjaName and EquipmentName
            builder.Entity<NinjaInventory>()
                .HasKey(ni => new { ni.NinjaName, ni.EquipmentName });

            // Configure many-to-many relationship between Ninja and Equipment using the foreign keys
            builder.Entity<NinjaInventory>()
                .HasOne(ni => ni.Ninja)
                .WithMany(n => n.NinjaInventories)
                .HasForeignKey(ni => ni.NinjaName); // Foreign key for Ninja

            builder.Entity<NinjaInventory>()
                .HasOne(ni => ni.Equipment)
                .WithMany(e => e.NinjaInventories)
                .HasForeignKey(ni => ni.EquipmentName); // Foreign key for Equipment
        }

        private static void SeedData(ModelBuilder builder)
        {
            // Seed Ninja
            builder.Entity<Ninja>().HasData(
                new Ninja
                {
                    Name = "Erratic Ephemeron",
                    Gold = 2000,
                    MaxHeadEquipment = 1,
                    MaxHandEquipment = 1,
                    MaxFeetEquipment = 1,
                    MaxNecklaceEquipment = 1,
                    MaxChestEquipment = 1,
                    MaxRingEquipment = 1
                }
            );

            // Seed Equipment matching the image
            builder.Entity<Equipment>().HasData(
                new Equipment
                {
                    Name = "Deze Petje", EquipmentType = EquipmentType.Head, MonetaryValue = 200, Strength = 5,
                    Agility = 200, Intelligence = -200
                },
                new Equipment
                {
                    Name = "Spikey Gauntlets", EquipmentType = EquipmentType.Hand, MonetaryValue = 100,
                    Strength = 10, Agility = 20, Intelligence = 0
                },
                new Equipment
                {
                    Name = "Daedric Boots", EquipmentType = EquipmentType.Foot, MonetaryValue = 150,
                    Strength = 80, Agility = 5, Intelligence = 5
                },
                new Equipment
                {
                    Name = "Draugr Amulet", EquipmentType = EquipmentType.Necklace, MonetaryValue = 500,
                    Strength = 5, Agility = -20, Intelligence = 50
                },
                new Equipment
                {
                    Name = "Daedric Armor", EquipmentType = EquipmentType.Chest, MonetaryValue = 1000,
                    Strength = 150, Agility = -20, Intelligence = 45
                },
                new Equipment
                {
                    Name = "Wedding Band", EquipmentType = EquipmentType.Ring, MonetaryValue = 30, Strength = 0,
                    Agility = 300, Intelligence = -150
                }
            );

            // Seed NinjaInventory (linking table) to assign the equipment to the ninja
            builder.Entity<NinjaInventory>().HasData(
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Deze Petje" },
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Spikey Gauntlets" },
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Daedric Boots" },
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Draugr Amulet" },
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Daedric Armor" },
                new NinjaInventory
                    { NinjaName = "Erratic Ephemeron", EquipmentName = "Wedding Band" }
            );
        }
    }


    public class Ninja
    {
        public string Name { get; set; }
        public int Gold { get; set; }

        public int MaxHeadEquipment { get; set; }
        public int MaxHandEquipment { get; set; }
        public int MaxFeetEquipment { get; set; }
        public int MaxNecklaceEquipment { get; set; }
        public int MaxChestEquipment { get; set; }
        public int MaxRingEquipment { get; set; }
        
            
        public ICollection<NinjaInventory> NinjaInventories { get; set; } = new List<NinjaInventory>();

        public int GetMaxEquipmentOfType(EquipmentType equipmentType)
        {
            return equipmentType switch
            {
                EquipmentType.Head => MaxHeadEquipment,
                EquipmentType.Hand => MaxHandEquipment,
                EquipmentType.Foot => MaxFeetEquipment,
                EquipmentType.Necklace => MaxNecklaceEquipment,
                EquipmentType.Chest => MaxChestEquipment,
                EquipmentType.Ring => MaxRingEquipment,
                _ => 0
            };
        }
    }

    public class NinjaInventory
    {
        public Ninja Ninja { get; set; }
        public Equipment Equipment { get; set; }

        public string NinjaName { get; set; }
        public string EquipmentName { get; set; }
    }
    
    public enum EquipmentType
    {
        Head,
        Hand,
        Foot,
        Necklace,
        Chest,
        Ring
    }

    public class Equipment
    {
        // Set Name as the primary key
        public string Name { get; set; }

        public EquipmentType EquipmentType { get; set; }
        public int MonetaryValue { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }

        public ICollection<NinjaInventory> NinjaInventories { get; set; } = new List<NinjaInventory>();
    }

}