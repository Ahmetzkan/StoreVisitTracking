using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs.Photo;
using StoreVisitTracking.Application.DTOs.Visit;
using StoreVisitTracking.Application.Paginate;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using StoreVisitTracking.Infrastructure;

public class VisitService : IVisitService
{
    private readonly StoreVisitTrackingDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateVisitDto> _visitValidator;
    private readonly IValidator<PhotoDto> _photoValidator;

    public VisitService(StoreVisitTrackingDbContext context, IMapper mapper, IValidator<CreateVisitDto> visitValidator, IValidator<PhotoDto> photoValidator)
    {
        _context = context;
        _mapper = mapper;
        _visitValidator = visitValidator;
        _photoValidator = photoValidator;
    }

    public async Task CreateAsync(Guid userId, CreateVisitDto dto)
    {
        await _visitValidator.ValidateAndThrowAsync(dto);
        var visit = _mapper.Map<Visit>(dto);
        visit.UserId = userId;
        visit.Id = Guid.NewGuid();
        visit.VisitDate = DateTime.UtcNow;
        visit.Status = VisitStatus.InProgress;

        _context.Visits.Add(visit);
        await _context.SaveChangesAsync();
    }

    public async Task<IPaginate<GetVisitDto>> GetAllAsync(Guid userId, bool isAdmin, PageRequest pageRequest)
    {
        var query = _context.Visits
            .Include(v => v.Store)
            .Include(v => v.Photos)
                .ThenInclude(p => p.Product)
            .AsQueryable();

        if (!isAdmin) { query = query.Where(v => v.UserId == userId); }

        var result = await query
            .OrderByDescending(v => v.VisitDate)
            .ToPaginateAsync(pageRequest.PageIndex, pageRequest.PageSize);

        return result.MapPaginate<Visit, GetVisitDto>(_mapper);
    }


    public async Task<GetVisitDto> GetByIdAsync(Guid userId, Guid visitId, bool isAdmin)
    {
        var visit = await _context.Visits
            .Include(v => v.Store)
            .Include(v => v.Photos)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(v => v.Id == visitId);

        if (visit == null) throw new Exception("Visit not found");

        if (!isAdmin && visit.UserId != userId) throw new UnauthorizedAccessException();

        return _mapper.Map<GetVisitDto>(visit);
    }

    public async Task<IPaginate<GetVisitDto>> GetUserVisitsAsync(Guid userId, PageRequest pageRequest)
    {
        var visits = await _context.Visits
            .Include(v => v.Store)
            .Include(v => v.Photos)
                .ThenInclude(p => p.Product)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.VisitDate)
            .ToPaginateAsync(pageRequest.PageIndex, pageRequest.PageSize);

        return visits.MapPaginate<Visit, GetVisitDto>(_mapper);
    }

    public async Task UploadPhotoAsync(Guid userId, Guid visitId, PhotoDto photoDto)
    {
        await _photoValidator.ValidateAndThrowAsync(photoDto);
        var visit = await _context.Visits.FindAsync(visitId);
        if (visit == null || visit.UserId != userId) throw new UnauthorizedAccessException();

        var photo = _mapper.Map<Photo>(photoDto);
        photo.Id = Guid.NewGuid();
        photo.VisitId = visitId;
        photo.UploadedAt = DateTime.UtcNow;

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteVisitAsync(Guid userId, Guid visitId)
    {
        var visit = await _context.Visits.FindAsync(visitId);
        if (visit == null || visit.UserId != userId) throw new UnauthorizedAccessException();

        visit.Status = VisitStatus.Completed;
        await _context.SaveChangesAsync();
    }
}
