using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShopNameCaseInsensetive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE UNIQUE INDEX "IX_Shops_Name_CaseInsensitive_Active"
                ON "Shops" (LOWER("Name"))
                WHERE "DeletedAt" IS NULL;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP INDEX "IX_Shops_Name_CaseInsensitive_Active";
                """
            );
        }
    }
}
