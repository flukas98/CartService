using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CartId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), null, "Professional tennis racket", "Pro Racket", 149.99m },
                    { new Guid("11111111-1111-1111-1111-111111111102"), null, "Lightweight racket for juniors", "Junior Racket", 59.99m },
                    { new Guid("22222222-2222-2222-2222-222222222201"), null, "Regulation tennis ball", "Championship Tennis Ball", 2.99m },
                    { new Guid("22222222-2222-2222-2222-222222222202"), null, "Practice-grade tennis ball", "Practice Tennis Ball", 1.99m },
                    { new Guid("33333333-3333-3333-3333-333333333301"), null, "Tennis court shoes", "Court Runner", 89.99m },
                    { new Guid("33333333-3333-3333-3333-333333333302"), null, "High-grip tennis shoes", "Grip Master", 99.99m }
                });

            migrationBuilder.InsertData(
                table: "Balls",
                columns: new[] { "Id", "Diameter", "Material" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222201"), 6.7000000000000002, "Rubber/Felt" },
                    { new Guid("22222222-2222-2222-2222-222222222202"), 6.7000000000000002, "Rubber/Felt" }
                });

            migrationBuilder.InsertData(
                table: "Rackets",
                columns: new[] { "Id", "GripSize", "HeadSize" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), "L2", 645.0 },
                    { new Guid("11111111-1111-1111-1111-111111111102"), "L0", 600.0 }
                });

            migrationBuilder.InsertData(
                table: "Shoes",
                columns: new[] { "Id", "Brand", "Size" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), "SportX", 42 },
                    { new Guid("33333333-3333-3333-3333-333333333302"), "SportX", 43 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Balls",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222201"));

            migrationBuilder.DeleteData(
                table: "Balls",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222202"));

            migrationBuilder.DeleteData(
                table: "Rackets",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"));

            migrationBuilder.DeleteData(
                table: "Rackets",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"));

            migrationBuilder.DeleteData(
                table: "Shoes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"));

            migrationBuilder.DeleteData(
                table: "Shoes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222201"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222202"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"));
        }
    }
}
