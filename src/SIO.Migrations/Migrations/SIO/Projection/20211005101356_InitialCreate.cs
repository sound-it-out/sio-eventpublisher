using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SIO.Migrations.Migrations.SIO.Projection
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventPublicationFailure",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPublicationFailure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventPublicationQueue",
                columns: table => new
                {
                    Subject = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPublicationQueue", x => x.Subject);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionState",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionState", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventPublicationFailure");

            migrationBuilder.DropTable(
                name: "EventPublicationQueue");

            migrationBuilder.DropTable(
                name: "ProjectionState");
        }
    }
}
