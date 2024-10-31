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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentType = table.Column<int>(type: "int", nullable: false),
                    MonetaryValue = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ninjas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ninjas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NinjaInventories",
                columns: table => new
                {
                    NinjaId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    NinjaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaInventories", x => new { x.NinjaId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_NinjaInventories_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NinjaInventories_Ninjas_NinjaId",
                        column: x => x.NinjaId,
                        principalTable: "Ninjas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Agility", "EquipmentType", "Intelligence", "MonetaryValue", "Name", "Strength" },
                values: new object[,]
                {
                    { 1, 10, 1, 5, 100, "Katana", 15 },
                    { 2, 15, 1, 0, 50, "Shuriken", 5 },
                    { 3, 5, 1, 0, 200, "Ninja Armor", 10 }
                });

            migrationBuilder.InsertData(
                table: "Ninjas",
                columns: new[] { "Id", "Gold", "Name" },
                values: new object[,]
                {
                    { 1, 100, "Shadow" },
                    { 2, 150, "Blade" }
                });

            migrationBuilder.InsertData(
                table: "NinjaInventories",
                columns: new[] { "EquipmentId", "NinjaId", "EquipmentName", "NinjaName" },
                values: new object[,]
                {
                    { 1, 1, "Katana", "Shadow" },
                    { 2, 1, "Shuriken", "Shadow" },
                    { 3, 2, "Ninja Armor", "Blade" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NinjaInventories_EquipmentId",
                table: "NinjaInventories",
                column: "EquipmentId");
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
