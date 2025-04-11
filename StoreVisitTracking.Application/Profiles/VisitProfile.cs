using AutoMapper;
using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Profiles
{
    public class VisitProfile : Profile
    {
        public VisitProfile()
        {
            CreateMap<Visit, GetVisitDto>()
                       .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                       .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateVisitDto, Visit>()
           .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => DateTime.UtcNow))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => VisitStatus.InProgress));

            CreateMap<Visit, GetVisitDto>()
           .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos));
        }
    }
}
