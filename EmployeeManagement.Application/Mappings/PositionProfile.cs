using AutoMapper;
using EmployeeManagement.Application.DTOs.Position;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Mappings
{
    class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<CreatePositionDto, Position>();
            CreateMap<UpdatePositionDto, Position>();
        }
    }
}
