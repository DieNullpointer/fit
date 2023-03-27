using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
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

        public PackageDto GetAllEvents
    }
}
