﻿using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            var company = await _db.Companies.Include(c=>c.ContactPartners).FirstAsync(a => a.Guid == guid);
            if (company is null)
                throw new ServiceException("Firma gibt es nicht");

            _db.ContactPartners.RemoveRange(company.ContactPartners.ToList());
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
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
            var packages = await _db.Packages.FirstAsync(a => a.Guid == register.packageGuid);
            var events = await _db.Events.Include(a => a.Packages).FirstAsync(a => a.Guid == register.eventGuid);
            if (!events.Packages.Contains(packages))
                throw new ServiceException("Package stimmt mit Event nicht überein");
            var company = _mapper.Map<Company>(register, opt => opt.AfterMap((dto, entity) =>
            {
                entity.Event = events;
                entity.Package = packages;
            }));
            //if (company is null) throw new ServiceException("Firma nicht gültig");

            //var package = await _db.Packages.FirstAsync(a => a.Name == register.package);
            //var events = await _db.Events.FirstAsync(a => a.Name == register.eventName);
            //var company = new Company(register.name, register.address, register.country, register.plz, register.place, register.billAddress, package, events);
            if(register.partners.Where(a => a.mainPartner).Count() > 1) { throw new ServiceException("Nur ein Hauptansprechpartner erlaubt"); }
            foreach (var i in register.partners)
            {
                await _db.ContactPartners.AddAsync(new ContactPartner(firstname: i.firstname, lastname: i.lastname, email: i.email, telNr: i.telNr, function: i.function, company: company, title: i.title, mobilNr: i.mobilNr, mainPartner: i.mainPartner));
            }
            await _db.Companies.AddAsync(company);
            try
            {
                await _db.SaveChangesAsync();
                return company.Guid;
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
        }

        public async Task<Guid> EditCompany(CompanyDto companydto)
        {
            var company = await _db.Companies.FirstAsync(a => a.Guid == companydto.guid);
            if (company is null)
            {
                throw new ServiceException($"Firma {companydto.guid} existiert nicht");
            }
            company.Name = companydto.name;
            company.Address = companydto.address;
            company.Country = companydto.country;
            company.Plz = companydto.plz;
            company.Place = companydto.place;
            company.BillAddress = companydto.billAddress;
            company.HasPaid = companydto.hasPaid;
            try
            {
                await _db.SaveChangesAsync();
                return company.Guid;
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
        }

        public async Task<Guid> AddDescription(string description, Guid guid)
        {
            var company = await _db.Companies.FirstAsync(a => a.Guid == guid);
            if (company is null)
                throw new ServiceException($"Firma {guid} existiert nicht");
            company.Description = description;
            try
            {
                await _db.SaveChangesAsync();
                return company.Guid;
            }
            catch (DbUpdateException e) { throw new ServiceException(e.InnerException?.Message ?? e.Message, e); }
        }
    }
}
