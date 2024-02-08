using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Genres(Name, UrlImage) " +
                "VALUES ('Adventure', 'adventure.jpg')");
            mb.Sql("INSERT INTO Genres(Name, UrlImage) " +
                "VALUES ('Science Fiction', 'science.jpg')");
            mb.Sql("INSERT INTO Genres(Name, UrlImage) " +
                "VALUES ('Fantasy', 'fantasy.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Genres");
        }
    }
}
