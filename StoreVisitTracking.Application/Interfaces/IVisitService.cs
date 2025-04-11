using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using System.Security.Claims;

public interface IVisitService
{
    Task CreateAsync(Guid userId, CreateVisitDto dto);
    Task<IPaginate<GetVisitDto>> GetAllAsync(Guid userId, bool isAdmin, PageRequest pageRequest);
    Task<GetVisitDto> GetByIdAsync(Guid userId, Guid visitId, bool isAdmin);
    Task UploadPhotoAsync(Guid userId, Guid visitId, PhotoDto photo);
    Task CompleteVisitAsync(Guid userId, Guid visitId);
}
