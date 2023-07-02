using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class CreateTablesAndSeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Open" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "In Progress" });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Done" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "BoardId", "CreatedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { new Guid("036cad15-6289-4b2a-8bc2-b2739c1c1d03"), 3, new DateTime(2022, 6, 16, 15, 12, 50, 32, DateTimeKind.Utc).AddTicks(8971), "Implement [Create Task] page for adding tasks", "9881bc74-6d04-4a05-b2d8-9b083654488c", "Create Tasks" },
                    { new Guid("aa3324de-2c66-49b8-bf16-b13dbbffce9b"), 1, new DateTime(2022, 11, 28, 15, 12, 50, 32, DateTimeKind.Utc).AddTicks(8809), "Implement better styling for all public pages", "9881bc74-6d04-4a05-b2d8-9b083654488c", "Implement CSS styles" },
                    { new Guid("cc24a34e-4aa8-4d32-8a26-9ad958d1d7f5"), 1, new DateTime(2023, 1, 16, 15, 12, 50, 32, DateTimeKind.Utc).AddTicks(8945), "Create Android client App for the RESTful TaskBoard service", "992c51bd-f5ec-4a3d-9ec1-8152eaeff526", "Android Client App" },
                    { new Guid("fb7973d9-85de-4f0c-b0a8-1b81bd44f128"), 2, new DateTime(2023, 5, 16, 15, 12, 50, 32, DateTimeKind.Utc).AddTicks(8964), "Create Desktop client App for the RESTful TaskBoard service", "9881bc74-6d04-4a05-b2d8-9b083654488c", "Desktop Client App" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BoardId",
                table: "Tasks",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
