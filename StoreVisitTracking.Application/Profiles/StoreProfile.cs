using AutoMapper;
using StoreVisitTracking.Application.DTOs;
using StoreVisitTracking.Application.DTOs.Store;
using StoreVisitTracking.Domain.Entities;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, GetAllStoreDto>().ReverseMap();
        CreateMap<Store, CreateStoreDto>().ReverseMap();
        CreateMap<Store, UpdateStoreDto>().ReverseMap();
    }
}
