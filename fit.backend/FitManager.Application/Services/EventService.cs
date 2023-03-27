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

        //All Events in the future
        public async Task<List<AllEventDto>> GetAllEvents()
        {
            var events = await _db.Events.Where(a => a.Date > DateTime.UtcNow.Date).OrderBy(a => a.Date).ToListAsync();
            if (events is null || events.Count == 0)
                throw new ServiceException("Keine Events in der Zukunft");
            var export = _mapper.Map<List<Event>, List<AllEventDto>>(events);
            return export;
        }
    }
}
