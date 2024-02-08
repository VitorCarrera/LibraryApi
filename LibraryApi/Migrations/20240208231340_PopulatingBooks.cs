using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApi.Migrations
{
    /// <inheritdoc />
    public partial class PopulatingBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Books (Name, Description, Price, UrlImage, Stock, RegistrationDate, GenreId) " +
                "VALUES ('O Último Olimpiano', 'Aventura em alta voltagem e impossível de largar', 80.00, 'percy.jpg', 10, now(), 1)");
            mb.Sql("INSERT INTO Books (Name, Description, Price, UrlImage, Stock, RegistrationDate, GenreId) " +
                "VALUES ('ACOTAR', 'A história gira em torno de Feyre Archeron, uma jovem caçadora, que acaba sendo arrastada para um reino mágico após matar um lobo na floresta, que na verdade era uma criatura feérica disfarçada. ', 120.00, 'acotar.jpg', 20, now(), 3)");
            mb.Sql("INSERT INTO Books (Name, Description, Price, UrlImage, Stock, RegistrationDate, GenreId) " +
                "VALUES ('O Guia do Mochileiro das Galáxias', 'Uma comédia espacial que segue as aventuras absurdas de Arthur Dent após a destruição da Terra.', 99.00, 'guia.jpg', 15, now(), 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Books");
        }
    }
}
