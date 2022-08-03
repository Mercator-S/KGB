using AutoMapper;
using KGB_Models.KGB_Model;

namespace KGB_Dev_.Data.Profiles
{
    public class KGB_CategoryProfile : Profile
    {
        public KGB_CategoryProfile()
        {
            CreateMap<KGB_CategoryViewModel, KGB_Category>()
                .ForMember(dest => dest.d_ins,
                           opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.d_upd,
                           opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<KGB_User, KGB_Category>()
               .ForMember(dest => dest.Id,
                           opt => opt.Ignore())
               .ForMember(dest => dest.d_ins,
                           opt => opt.Ignore())
               .ForMember(dest => dest.d_upd,
                           opt => opt.Ignore())
               .ForMember(dest => dest.k_ins,
                           opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.k_upd,
                           opt => opt.MapFrom(src => src.Id));
        }
    }
}
