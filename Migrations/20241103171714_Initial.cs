using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PROG5_NinjaManager.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentType = table.Column<int>(type: "int", nullable: false),
                    MonetaryValue = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Ninjas",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    MaxHeadEquipment = table.Column<int>(type: "int", nullable: false),
                    MaxHandEquipment = table.Column<int>(type: "int", nullable: false),
                    MaxFeetEquipment = table.Column<int>(type: "int", nullable: false),
                    MaxNecklaceEquipment = table.Column<int>(type: "int", nullable: false),
                    MaxChestEquipment = table.Column<int>(type: "int", nullable: false),
                    MaxRingEquipment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ninjas", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "NinjaInventories",
                columns: table => new
                {
                    NinjaName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaInventories", x => new { x.NinjaName, x.EquipmentName });
                    table.ForeignKey(
                        name: "FK_NinjaInventories_Equipments_EquipmentName",
                        column: x => x.EquipmentName,
                        principalTable: "Equipments",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NinjaInventories_Ninjas_NinjaName",
                        column: x => x.NinjaName,
                        principalTable: "Ninjas",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Name", "Agility", "EquipmentType", "Intelligence", "MonetaryValue", "Strength" },
                values: new object[,]
                {
                    { "Daedric Armor", -20, 4, 45, 1000, 150 },
                    { "Daedric Boots", 5, 2, 5, 150, 80 },
                    { "Deze Petje", 200, 0, -200, 200, 5 },
                    { "Draugr Amulet", -20, 3, 50, 500, 5 },
                    { "Spikey Gauntlets", 20, 1, 0, 100, 10 },
                    { "Wedding Band", 300, 5, -150, 30, 0 }
                });

            migrationBuilder.InsertData(
                table: "Ninjas",
                columns: new[] { "Name", "Gold", "MaxChestEquipment", "MaxFeetEquipment", "MaxHandEquipment", "MaxHeadEquipment", "MaxNecklaceEquipment", "MaxRingEquipment" },
                values: new object[] { "Erratic Ephemeron", 2000, 1, 1, 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "NinjaInventories",
                columns: new[] { "EquipmentName", "NinjaName" },
                values: new object[,]
                {
                    { "Daedric Armor", "Erratic Ephemeron" },
                    { "Daedric Boots", "Erratic Ephemeron" },
                    { "Deze Petje", "Erratic Ephemeron" },
                    { "Draugr Amulet", "Erratic Ephemeron" },
                    { "Spikey Gauntlets", "Erratic Ephemeron" },
                    { "Wedding Band", "Erratic Ephemeron" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NinjaInventories_EquipmentName",
                table: "NinjaInventories",
                column: "EquipmentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NinjaInventories");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Ninjas");
        }
    }
}
