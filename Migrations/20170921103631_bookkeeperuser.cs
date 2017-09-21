using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookKeeperSPAAngular.Migrations
{
    public partial class bookkeeperuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookKeeperUserId",
                table: "BookKeeper",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookKeeperUser",
                columns: table => new
                {
                    BookKeeperUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookKeeperUser", x => x.BookKeeperUserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookKeeper_BookKeeperUserId",
                table: "BookKeeper",
                column: "BookKeeperUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookKeeper_BookKeeperUser_BookKeeperUserId",
                table: "BookKeeper",
                column: "BookKeeperUserId",
                principalTable: "BookKeeperUser",
                principalColumn: "BookKeeperUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookKeeper_BookKeeperUser_BookKeeperUserId",
                table: "BookKeeper");

            migrationBuilder.DropTable(
                name: "BookKeeperUser");

            migrationBuilder.DropIndex(
                name: "IX_BookKeeper_BookKeeperUserId",
                table: "BookKeeper");

            migrationBuilder.DropColumn(
                name: "BookKeeperUserId",
                table: "BookKeeper");
        }
    }
}
