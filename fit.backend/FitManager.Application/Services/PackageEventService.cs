using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public IQueryable<Package> Packages => _db.Packages.AsQueryable();
        public IQueryable<Event> Events => _db.Events.AsQueryable();

        public PackageEventService(IMapper mapper, FitContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        //  EVENT METHODS
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

        public async Task<bool> DeleteEvent(Guid guid)
        {
            var events = await _db.Events.Include(a => a.Packages).Include(a => a.Companies).FirstAsync(a => a.Guid == guid);
            if (events is null) throw new ServiceException("Event existiert nicht");
            events.Packages.Clear();
            _db.Companies.RemoveRange(events.Companies.ToList());
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            _db.Events.Remove(events);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return true;
        }

        public async Task<bool> AssignPackages(AssignPackageCmd packages)
        {
            if (packages.Packages.Count == 0) throw new ServiceException("Keine Pakete gesendet");
            var events = await _db.Events.FirstAsync(a => a.Guid == packages.EventGuid);
            if (events is null) { throw new ServiceException("Event existiert nicht"); }

            foreach (var package in packages.Packages)
            {
                var p = await _db.Packages.FirstAsync(a => a.Guid == package);
                if (p is not null)
                {
                    if (events.Packages.Exists(a => a.Guid == package))
                        throw new ServiceException("Mindestens ein Paket gibt es bereits");
                    events.Packages.Add(p);
                }
                else { throw new ServiceException("Mindestens ein Paket gibt es nicht"); }
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return true;
        }

        //  PACKAGE METHODS
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
