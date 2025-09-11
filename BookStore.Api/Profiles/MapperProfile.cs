

using AutoMapper;
using BookStore.Api.Dtos;
using BookStore.Api.Dtos.Author;
using BookStore.Api.Dtos.BooksDto;
using BookStore.Api.Dtos.Category;
using BookStore.Api.Dtos.OrderDto;
using BookStore.Api.Dtos.Publisher;
using BookStore.Api.Helpers;
using BookStore.Core.Models;
using BookStore.Core.Specification.Projections;

namespace BookStore.Api.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddressDto, UserAddress>().ReverseMap();
            CreateMap<UserDto, AppUser>()
                .ForMember(D => D.Id, O => O.MapFrom(S => S.UserId))
                .ReverseMap();

            CreateMap<Author, AuthorDto>()
                .ForMember(D => D.Books, O => O.MapFrom(S => S.Books.Select(B => B.Name)));

            CreateMap<Publisher, PublisherDto>()
                .ForMember(D => D.Books, O => O.MapFrom(S => S.Books.Select(B => B.Name)));

            CreateMap<Category, CategoryDto>()
                .ForMember(D => D.Books, O => O.MapFrom(S => S.Books.Select(B => B.Name)));

            CreateMap<BookProjection, BookDto>()
                .ForMember(D => D.CoverUrl, O => O.MapFrom<BookCoverUrlResolver>());

            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(D => D.UnitPice, O => O.MapFrom(S=>S.Price));

        }
    }
}
