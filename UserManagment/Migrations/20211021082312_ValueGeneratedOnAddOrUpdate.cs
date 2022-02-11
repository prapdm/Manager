using Microsoft.EntityFrameworkCore.Migrations;

namespace Manager.Migrations
{
    public partial class ValueGeneratedOnAddOrUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"CREATE TRIGGER [dbo].[Users_UPDATE] ON [dbo].[Users]
                AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

                DECLARE @Id INT

                SELECT @Id = INSERTED.Id
                FROM INSERTED

                UPDATE dbo.Users
                SET UpdatedAt = GETDATE()
                WHERE Id = @Id
            END");

            migrationBuilder.Sql(
            @"CREATE TRIGGER [dbo].[Roles_UPDATE] ON [dbo].[Roles]
                AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

                DECLARE @Id INT

                SELECT @Id = INSERTED.Id
                FROM INSERTED

                UPDATE dbo.Roles
                SET UpdatedAt = GETDATE()
                WHERE Id = @Id
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
