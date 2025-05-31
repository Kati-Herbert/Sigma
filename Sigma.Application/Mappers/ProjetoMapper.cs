using AutoMapper;
using Sigma.Domain.Dtos;
using Sigma.Domain.Entities;

namespace Sigma.Application.Mappers
{
    public class ProjetoMapper : Profile
    {
        public ProjetoMapper()
        {
            CreateMap<ProjetoNovoDto, Projeto>();
        }
    }
}


