using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CityCRUD.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    lastname = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    birthday = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_companies",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    name_of_responsible_person = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    address = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    id_city = table.Column<int>(nullable: false),
                    phone = table.Column<string>(unicode: false, maxLength: 11, nullable: false),
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    INN = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_companies", x => x.id);
                    table.ForeignKey(
                        name: "FK__delivery___id_ci__4222D4EF",
                        column: x => x.id_city,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publishing_houses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    id_city = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishing_houses", x => x.id);
                    table.ForeignKey(
                        name: "FK__publishin__id_ci__3B75D760",
                        column: x => x.id_city,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    pages = table.Column<int>(nullable: false),
                    id_author = table.Column<int>(nullable: false),
                    id_publishing_house = table.Column<int>(nullable: false),
                    cost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                    table.ForeignKey(
                        name: "FK__books__id_author__3E52440B",
                        column: x => x.id_author,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__books__id_publis__3F466844",
                        column: x => x.id_publishing_house,
                        principalTable: "publishing_houses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchases",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_book = table.Column<int>(nullable: false),
                    id_delivery_company = table.Column<int>(nullable: false),
                    time_of_purchase = table.Column<DateTime>(type: "datetime", nullable: false),
                    amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchases", x => x.id);
                    table.ForeignKey(
                        name: "FK__purchases__id_bo__44FF419A",
                        column: x => x.id_book,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__purchases__id_de__45F365D3",
                        column: x => x.id_delivery_company,
                        principalTable: "delivery_companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_books_id_author",
                table: "books",
                column: "id_author");

            migrationBuilder.CreateIndex(
                name: "IX_books_id_publishing_house",
                table: "books",
                column: "id_publishing_house");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_companies_id_city",
                table: "delivery_companies",
                column: "id_city");

            migrationBuilder.CreateIndex(
                name: "IX_publishing_houses_id_city",
                table: "publishing_houses",
                column: "id_city");

            migrationBuilder.CreateIndex(
                name: "IX_purchases_id_book",
                table: "purchases",
                column: "id_book");

            migrationBuilder.CreateIndex(
                name: "IX_purchases_id_delivery_company",
                table: "purchases",
                column: "id_delivery_company");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchases");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "delivery_companies");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "publishing_houses");

            migrationBuilder.DropTable(
                name: "cities");
        }
    }
}
