using AutoMapper;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using System;

namespace StoreVisitTracking.Application.Profiles
{
    public class VisitProfile : Profile
    {
        public VisitProfile()
        {
            CreateMap<Visit, GetVisitDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos));

            CreateMap<CreateVisitDto, Visit>()
                .ForMember(dest => dest.VisitDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => VisitStatus.InProgress));
        }
    }
}