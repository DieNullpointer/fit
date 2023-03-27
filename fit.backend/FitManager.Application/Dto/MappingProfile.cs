﻿using AutoMapper;
using FitManager.Application.Model;
using System.Diagnostics.Tracing;

namespace FitManager.Application.Dto
{
    public class MappingProfile : Profile  // using AutoMapper;
    {
        public MappingProfile()
        {
            CreateMap<CompanyCmd, Company>(); //CompanyCmd --> Company
            CreateMap<EventCmd, Event>(); //EventCmd --> Event
            CreateMap<PackageCmd, Package>(); //PackageCmd --> Package
            CreateMap<Event, AllEventDto>().ForMember(dest => dest.Date, act => act.MapFrom(src => (src.Date.ToString("dd/MM/yyyy")))); //Event --> AllEventDto
        }
    }
}
