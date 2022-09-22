using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;

namespace DatingApp.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(
                    x => x.Gender,
                    member => member.MapFrom(y => y.Gender == Gender.Male ? "Male" : "Female"))
                .ForMember(
                    x => x.PhotoUrl,
                    member => member.MapFrom(y => y.Photos != null && y.Photos.Any(x => x.IsMain) ? y.Photos.First(x => x.IsMain).Url : default
                ))
                .ForMember(
                    x => x.Age,
                    member => member.MapFrom(y => y.Dob.HowManyYearsItsBeen())
                )
                .ForMember(
                    x => x.CreatedAt,
                    member => member.MapFrom(y => y.CreatedAt.DateTime)
                );

            CreateMap<MemberDto, AppUser>()
                .ForMember(
                x => x.Gender,
                member => member.MapFrom(y => y.Gender == "Male" ? Gender.Male : Gender.Female));

            CreateMap<Photo, PhotoDto>().ReverseMap();

            CreateMap<UpdateMemberInputModel, AppUser>();
        }
    }
}