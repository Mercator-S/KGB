using AutoMapper;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;

namespace KGB_Dev_.Data.Profiles
{
    public class KGB_KnowledgeProfile : Profile
    {
        public KGB_KnowledgeProfile()
        {
            Random rd = new Random();
            CreateMap<KGB_KnowledgeViewModel, KGB_Knowledge>()
        .ForMember(dest => dest.d_ins,
               opt => opt.MapFrom(src => DateTime.Now))
        .ForMember(dest => dest.d_upd,
               opt => opt.MapFrom(src => DateTime.Now))
        .ForMember(dest => dest.Sifra_Prijave,
               opt => opt.MapFrom(src => src.Naziv_Prijave.Substring(0, 2)+rd.Next(100,999)));
        }
    }
}
