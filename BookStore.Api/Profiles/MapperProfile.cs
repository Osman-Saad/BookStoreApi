

using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Core.Models;

namespace BookStore.Api.Profiles
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<AddressDto,UserAddress>().ReverseMap();
            CreateMap<UserDto,AppUser>()
                .ForMember(D=>D.Id,O=>O.MapFrom(S=>S.UserId))
                .ReverseMap();
        }
    }
}
