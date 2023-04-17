using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FitManager.Application.Services
{
    public class CompanyService
    {
        private readonly IMapper _mapper;
        private readonly FitContext _db;

        public CompanyService(IMapper mapper, FitContext db)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<bool> DeleteCompany(Guid guid)
        {
            var company = await _db.Companies.FirstAsync(a => a.Guid == guid);
            if (company is null)
                throw new ServiceException("Firma gibt es nicht");
            _db.Companies.Remove(company);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
            return true;
        }

        public async Task<Guid> AddCompany(CompanyCmd register)
        {
            var company = _mapper.Map<Company>(register, opt => opt.AfterMap(async (dto, entity) =>
            {
                entity.Event = await _db.Events.FirstAsync(a => a.Guid == register.eventGuid);
                entity.Package = await _db.Packages.FirstAsync(a => a.Guid == register.packageGuid);
            }));
            if (company.Event.Packages.Contains(company.Package))
                throw new ServiceException("Package stimmt mit Event nicht überein");
            //var package = await _db.Packages.FirstAsync(a => a.Name == register.package);
            //var events = await _db.Events.FirstAsync(a => a.Name == register.eventName);
            //var company = new Company(register.name, register.address, register.country, register.plz, register.place, register.billAddress, package, events);
            foreach (var i in register.partners)
            {
                await _db.ContactPartners.AddAsync(new ContactPartner(i.title, i.firstname, i.lastname, i.email, i.telNr, i.function, company, i.mobilNr, i.mainPartner));
            }
            await _db.Companies.AddAsync(company);
            try
            {
                await _db.SaveChangesAsync();
                return company.Guid;
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
        }
    }
}
