using AutoMapper;
using Sigma.Domain.Entities;
using Sigma.Application.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Projeto, ProjetoDto>();

        CreateMap<ProjetoDto, Projeto>();

    }
}
