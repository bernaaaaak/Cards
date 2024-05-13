using AutoMapper;
using Cards.Models.DTOs;

namespace Cards.Models.Domain
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<NumericsDTO, NumericData>().ReverseMap();
        }
    }
}

public class numericDto
    { 
public string Name {get; set;}
    }


public class NumericData
    {
public string Name {get; set;}
    }
