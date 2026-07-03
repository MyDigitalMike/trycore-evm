using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trycore.Evm.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    bac = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    planned_progress_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    actual_progress_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    actual_cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.id);
                    table.CheckConstraint("ck_activities_actual_cost", "actual_cost >= 0");
                    table.CheckConstraint("ck_activities_actual_percent", "actual_progress_percent >= 0 AND actual_progress_percent <= 100");
                    table.CheckConstraint("ck_activities_bac", "bac >= 0");
                    table.CheckConstraint("ck_activities_planned_percent", "planned_progress_percent >= 0 AND planned_progress_percent <= 100");
                    table.ForeignKey(
                        name: "FK_activities_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_activities_project_id",
                table: "activities",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
