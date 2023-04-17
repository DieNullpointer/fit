using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FitManager.Application.Services
{
    /**
     * Dieses Service ist für Event und Package
     * Die Methoden sind für deren Controller
     */
    public class PackageEventService
    {
        private readonly IMapper _mapper;
        private readonly FitContext _db;

        public PackageEventService(IMapper mapper, FitContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<Guid> AddEvent(EventCmd events)
        {
            if (events.Date < DateTime.UtcNow)
                throw new ServiceException("Datum liegt in der Vergangenheit");
            var ev = _mapper.Map<Event>(events);
            await _db.Events.AddAsync(ev);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return ev.Guid;
        }

        public async Task<Guid> AddPackage(PackageCmd cmd)
        {
            var package = _mapper.Map<Package>(cmd, opt => opt.AfterMap((dto, entity) =>
            {
                entity.Price = decimal.Parse(cmd.price);
            }));
            await _db.Packages.AddAsync(package);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return package.Guid;
        }
    }
}
