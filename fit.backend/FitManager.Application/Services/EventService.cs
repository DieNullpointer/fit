using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MimeKit.Encodings;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitManager.Application.Services
{
    public class EventService
    {
        private readonly IMapper _mapper;
        private readonly FitContext _db;

        public EventService(IMapper mapper, FitContext db)
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
    }
}
