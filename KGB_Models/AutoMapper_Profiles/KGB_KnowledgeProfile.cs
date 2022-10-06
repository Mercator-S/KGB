using AutoMapper;
using KGB_Models.KGB_Model;

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
               opt => opt.MapFrom(src => src.Naziv_Prijave.Substring(0, 2) + rd.Next(100, 999)));

            CreateMap<KGB_Knowledge, KGB_KnowledgeViewModel>();

            CreateMap<KGB_User, KGB_Knowledge>()
            .ForMember(dest => dest.Id,
               opt => opt.Ignore())
            .ForMember(dest => dest.d_ins,
               opt => opt.Ignore())
            .ForMember(dest => dest.d_upd,
               opt => opt.Ignore())
            .ForMember(dest => dest.Naziv_Oj,
               opt => opt.MapFrom(src => src.Naziv_Oj))
        .ForMember(dest => dest.Sifra_Oj,
               opt => opt.MapFrom(src => src.Sifra_Oj))
             .ForMember(dest => dest.k_name,
                   opt => opt.MapFrom(src => src.Ime + " " + src.Prezime))
            .ForMember(dest => dest.k_ins,
                   opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.k_upd,
                   opt => opt.MapFrom(src => src.Id));
        }
    }
}
