﻿using AutoMapper;
using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.Data.KGB_ViewModel;

namespace KGB_Dev_.Data.Profiles
{
    public class KGB_SubCategoryProfile:Profile
    {
        public KGB_SubCategoryProfile()
        {
            CreateMap<KGB_SubcategoryViewModel, KGB_Subcategory>()
               .ForMember(dest => dest.d_ins,
                          opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.d_upd,
                          opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<KGB_User, KGB_Subcategory>()
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
