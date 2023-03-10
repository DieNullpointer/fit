using AutoMapper;
using FitManager.Application.Model;

namespace FitManager.Application.Dto
{
    public class MappingProfile : Profile  // using AutoMapper;
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, Company>(); //RegisterDto --> Company
        }
    }
}
