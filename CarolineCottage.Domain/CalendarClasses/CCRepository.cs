using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CarolineCottage.Repository.CarolineCottageClasses;

namespace CarolineCottage.Domain
{
    public static class AutoMapperDomainConfiguration
    {
        public static void Configure(params string[] assemblies)
        {
            Mapper.Initialize(cfg =>
            {

                cfg.AddProfiles(assemblies);                

            });
            
        }
    }
    public class CCProfile : Profile
    {
        public CCProfile()
        {
            CreateMap<BookingView, Booking>()
                .ReverseMap();
            CreateMap<User, UserList>()
                .ReverseMap();
        }
    }
    public class CCRepository
    {
    }
}
