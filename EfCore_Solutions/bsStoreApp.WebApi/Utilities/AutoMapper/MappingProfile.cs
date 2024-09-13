using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bsStoreApp.WebApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<BookDtoForInsetion, Book>().ReverseMap();
        }

    }
}
