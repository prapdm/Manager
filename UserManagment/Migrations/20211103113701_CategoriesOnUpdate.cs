using Microsoft.EntityFrameworkCore.Migrations;

namespace Manager.Migrations
{
    public partial class CategoriesOnUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE TRIGGER [dbo].[Categories_UPDATE] ON [dbo].[Categories]
                AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

                DECLARE @Id INT

                SELECT @Id = INSERTED.Id
                FROM INSERTED

                UPDATE dbo.Categories
                SET UpdatedAt = GETDATE()
                WHERE Id = @Id
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
