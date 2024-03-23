using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace moviemartapi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "ActorId", "Name" },
                values: new object[,]
                {
                    { 1, "Actor1" },
                    { 2, "Actor2" },
                    { 3, "Actor3" }
                });

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "DirectorId", "Name" },
                values: new object[,]
                {
                    { 1, "Director1" },
                    { 2, "Director2" },
                    { 3, "Director3" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Drama" },
                    { 3, "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password", "Token", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "hashedPassword", "TempToken1", "admin" },
                    { 2, "user2@example.com", "hashedPassword2", "TempToken2", "user2" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MovieId", "Description", "DirectorId", "Duration", "GenreId", "ImageUrl", "Language", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, "Description for Movie1", 1, new TimeSpan(0, 2, 0, 0, 0), 1, "https://m.media-amazon.com/images/M/MV5BNzQxNzQxNjk5NV5BMl5BanBnXkFtZTgwNTI4MTU0MzE@._V1_.jpg", "English", new DateTime(2024, 3, 18, 20, 50, 44, 189, DateTimeKind.Local).AddTicks(2570), "IMAGE" },
                    { 2, "Description for Movie2", 2, new TimeSpan(0, 1, 50, 0, 0), 2, "https://resizing.flixster.com/aqOkZpMlbwsRHEU9tpnBr8845YM=/fit-in/180x240/v2/https://resizing.flixster.com/O6GxoETackXlsx2N9MJPIbp4-54=/ems.cHJkLWVtcy1hc3NldHMvbW92aWVzLzQwNmE0YjRkLTFiM2ItNDk4Yi1hZTFhLWJiMDI2OTM1NDAyMy5qcGc=", "English", new DateTime(2024, 3, 18, 20, 50, 44, 189, DateTimeKind.Local).AddTicks(2630), "TILL" },
                    { 3, "Description for Movie 3", 3, new TimeSpan(0, 2, 10, 0, 0), 3, "https://m.media-amazon.com/images/I/81wR1ScI7nL._AC_UF1000,1000_QL80_.jpg", "English", new DateTime(2024, 3, 18, 20, 50, 44, 189, DateTimeKind.Local).AddTicks(2640), "White Chicks" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "ActorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "ActorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "ActorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "MovieId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "MovieId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "MovieId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "DirectorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "DirectorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "DirectorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "GenreId",
                keyValue: 3);
        }
    }
}
