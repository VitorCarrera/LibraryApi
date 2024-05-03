using LibraryApi.Models;

namespace LibraryApi.DTOs.Mappings;

public static class GenreDTOMappingExtensions
{

    public static GenreDTO? ToGenreDTO(this Genre genre)
    {
        if (genre is null)
            return null;

        return new GenreDTO
        {
            GenreId = genre.GenreId,
            Name = genre.Name,
            UrlImage = genre.UrlImage
        };
    }

    public static Genre? ToGenre(this GenreDTO genreDTO)
    {
        if(genreDTO is null) 
            return null;

        return new Genre
        {
            GenreId = genreDTO.GenreId,
            Name = genreDTO.Name,
            UrlImage = genreDTO.UrlImage
        };
    }


    public static IEnumerable<GenreDTO> ToGenreDTOList(this IEnumerable<Genre> genres)
    {
        if (genres is null || !genres.Any())
            return new List<GenreDTO>();

        return genres.Select(genre => new GenreDTO
        {
            GenreId = genre.GenreId,
            Name = genre.Name,
            UrlImage = genre.UrlImage
        }).ToList();
    }
}
