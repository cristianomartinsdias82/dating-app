using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using DatingApp.Api.InputModels;

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
                    member => member.MapFrom(y => Enum.Parse<Gender>(y.Gender, true))
                );

            CreateMap<Photo, PhotoDto>().ReverseMap();

            CreateMap<UpdateMemberInputModel, AppUser>();

            CreateMap<RegisterUserInputModel, AppUser>()
                .ForMember(
                    x => x.Gender,
                    member => member.MapFrom(y => Enum.Parse<Gender>(y.Gender, true))
                );

            CreateMap<Message, MessageDto>()
                .ForMember(x => x.SenderPhotoUrl, x => x.MapFrom(y => y.Sender.Photos != null && y.Sender.Photos.Any(x => x.IsMain) ? y.Sender.Photos.First(y => y.IsMain).Url : default))
                .ForMember(x => x.RecipientPhotoUrl, x => x.MapFrom(y => y.Recipient.Photos != null && y.Recipient.Photos.Any(x => x.IsMain) ? y.Recipient.Photos.First(y => y.IsMain).Url : default))
                .ForMember(x => x.RecipientKnownAs, x => x.MapFrom(x => x.Recipient.KnownAs))
                .ForMember(x => x.RecipientUserName, x => x.MapFrom(x => x.Recipient.UserName))
                .ForMember(x => x.SenderKnownAs, x => x.MapFrom(x => x.Sender.KnownAs))
                .ForMember(x => x.SenderUserName, x => x.MapFrom(x => x.Sender.UserName));

        }
    }
}