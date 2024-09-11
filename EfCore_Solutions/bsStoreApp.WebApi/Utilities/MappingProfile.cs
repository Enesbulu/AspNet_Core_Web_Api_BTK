using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bsStoreApp.WebApi.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();
        }

    }
}
