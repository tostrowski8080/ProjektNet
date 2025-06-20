﻿using Microsoft.EntityFrameworkCore;
using WorkshopManager.Data;
using WorkshopManager.DTOs;
using WorkshopManager.Mappers;
using WorkshopManager.Models;
using WorkshopManager.Services;
public class ServiceOrderService : IServiceOrderService
{
    private readonly WorkshopDbContext _context;
    private readonly IServiceOrderMapper _mapper;

    public ServiceOrderService(WorkshopDbContext context, IServiceOrderMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceOrderDto>> GetAllAsync(
        int? vehicleId = null, string? status = null)
    {
        var query = _context.ServiceOrders
            .AsQueryable();

        if (vehicleId.HasValue)
            query = query.Where(o => o.VehicleId == vehicleId.Value);

        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<ServiceOrder.StatusType>(status, out var st))
        {
            query = query.Where(o => o.Status == st);
        }

        var list = await query.ToListAsync();
        return list.Select(_mapper.ToDto);
    }

    public async Task<ServiceOrderDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<ServiceOrder>()
            .Include(o => o.Tasks)
            .FirstOrDefaultAsync(o => o.Id == id);
        return entity == null ? null : _mapper.ToDto(entity);
    }

    public async Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto)
    {
        var entity = _mapper.ToEntity(dto);
        entity.CreationDate = DateTime.UtcNow;
        entity.Status = ServiceOrder.StatusType.New;
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.ToDto(entity);
    }

    public async Task UpdateAsync(ServiceOrderUpdateDto dto)
    {
        var existing = await _context.ServiceOrders.FirstOrDefaultAsync(o => o.Id == dto.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Order {dto.Id} not found.");

        if (!string.IsNullOrWhiteSpace(dto.Status) &&
            Enum.TryParse<ServiceOrder.StatusType>(dto.Status, out var newStatus))
            existing.Status = newStatus;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            existing.Description = dto.Description;

        if (!string.IsNullOrWhiteSpace(dto.WorkerId))
            existing.WorkerId = dto.WorkerId;

        if (dto.VehicleId.HasValue)
        {
            var vehicleExists = await _context.Vehicles
                .AnyAsync(v => v.Id == dto.VehicleId.Value);

            if (!vehicleExists)
                throw new ArgumentException($"Vehicle with ID {dto.VehicleId.Value} does not exist.");

            existing.VehicleId = dto.VehicleId.Value;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Set<ServiceOrder>().FindAsync(id);
        if (existing != null)
        {
            _context.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}

namespace WorkshopManager.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly WorkshopDbContext _context;
        private readonly IServiceOrderMapper _mapper;

        public ServiceOrderService(WorkshopDbContext context, IServiceOrderMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceOrderDto>> GetAllAsync(
            int? vehicleId = null, string? status = null)
        {
            var query = _context.Set<ServiceOrder>()
                .AsQueryable();

            if (vehicleId.HasValue)
                query = query.Where(o => o.VehicleId == vehicleId.Value);

            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<ServiceOrder.StatusType>(status, out var st))
            {
                query = query.Where(o => o.Status == st);
            }

            var list = await query.ToListAsync();
            return list.Select(_mapper.ToDto);
        }

        public async Task<ServiceOrderDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<ServiceOrder>()
                .Include(o => o.Tasks)
                .FirstOrDefaultAsync(o => o.Id == id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public async Task<ServiceOrderDto> CreateAsync(ServiceOrderCreateDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            entity.CreationDate = DateTime.UtcNow;
            entity.Status = ServiceOrder.StatusType.New;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.ToDto(entity);
        }

        public async Task UpdateAsync(ServiceOrderUpdateDto dto)
        {
            var existing = await _context.Set<ServiceOrder>()
                .FirstOrDefaultAsync(o => o.Id == dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Order {dto.Id} not found.");

            if (Enum.TryParse<ServiceOrder.StatusType>(dto.Status, out var newStatus))
                existing.Status = newStatus;

            existing.Description = dto.Description;
            existing.WorkerId = dto.WorkerId;

            await _context.SaveChangesAsync();

            if (Enum.TryParse<ServiceOrder.StatusType>(dto.Status, out var st)
                && st == ServiceOrder.StatusType.Finished)
            {
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Set<ServiceOrder>().FindAsync(id);
            if (existing != null)
            {
                _context.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
    }
}
