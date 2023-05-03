using AutoMapper;
using Avionera.Interfaces;
using Avionera.Models;
using Avionera.Services;
using Avionera.ViewModels;

namespace Avionera.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            IImageConverter imageConverter = new ImageConverter();

            CreateMap<AppUser, UserViewModel>();

            CreateMap<UserViewModel, AppUser>()
                .ForMember(dest => dest.PasswordHash, op => op.Ignore());

            CreateMap<AppUser, UserEditAdminViewModel>()
                .ForMember(dest => dest.AdminPassword, op => op.Ignore());

            CreateMap<UserEditAdminViewModel, AppUser>();

            CreateMap<OfferCreateViewModel, Offer>()
                .ForMember(dest => dest.Image, d => d.Ignore());
            CreateMap<Offer, OfferViewModel>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(o => imageConverter.ByteArrayToImgUrl(o.Image)));
        }
    }
}
