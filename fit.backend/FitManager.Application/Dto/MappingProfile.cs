using AutoMapper;
using FitManager.Application.Model;

namespace FitManager.Application.Dto
{
    public class MappingProfile : Profile  // using AutoMapper;
    {
        public MappingProfile()
        {
            CreateMap<CompanyCmd, Company>(); //CompanyCmd --> Company
            CreateMap<EventCmd, Event>(); //EventCmd --> Event
            CreateMap<PackageCmd, Package>(); //PackageCmd --> Package
        }
    }
}
