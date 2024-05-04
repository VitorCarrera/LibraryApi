using AutoMapper;
using LibraryApi.Models;

namespace LibraryApi.DTOs.Mappings
{
    public class BookDTOMappingProfile : Profile
    {

        public BookDTOMappingProfile() {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Book, BookDTOUpdateRequest>().ReverseMap();
            CreateMap<Book, BookDTOUpdateResponse>().ReverseMap();
        }  
    }
}
