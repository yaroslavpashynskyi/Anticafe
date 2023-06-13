using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Hall, Hall>();
            CreateMap<Hall, HallDto>();
            CreateMap<Activity, ActivityDto>();
        }
    }
}