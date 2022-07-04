using AutoMapper;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;

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
        }
    }
}
