using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sigma.Application.Dtos;
using Sigma.Application.Interfaces;
using Sigma.Domain.Dtos;
using Sigma.Domain.Entities;
using Sigma.Domain.Enums;
using Sigma.Domain.Interfaces.Repositories;
using Sigma.Infra.Data.Context;


namespace Sigma.Application.Services
{
    public class ProjetoService : IProjetoService
    {
        private readonly IMapper _mapper;
        private readonly IProjetoRepository _projetoRepository;
        private readonly SigmaContext _context;

        public ProjetoService(IMapper mapper, IProjetoRepository projetoRepository, SigmaContext context)
        {
            _mapper = mapper;
            _projetoRepository = projetoRepository;
            _context = context;
        }

        public async Task<bool> Alterar(long id, ProjetoEditarDto dto)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);

            if (projeto == null)
                throw new KeyNotFoundException("Projeto não encontrado.");
            
            projeto.Nome = dto.Nome;
            projeto.Descricao = dto.Descricao;
            projeto.DataInicio = (DateTime)dto.DataInicio;
            projeto.PrevisaoTermino = (DateTime)dto.PrevisaoTermino;
            projeto.OrcamentoTotal = (decimal)dto.OrcamentoTotal;
            projeto.Risco = (Risco)dto.Risco;
            projeto.Status = (StatusProjeto)dto.Status;

            if (projeto.Status == StatusProjeto.Encerrado)
                projeto.DataRealTermino = DateTime.UtcNow;
            else
                projeto.DataRealTermino = null;

            await _projetoRepository.UpdateAsync(projeto);
            return true;
        }


        public async Task<IEnumerable<ProjetoDto>> ConsultarPorFiltro(string? nome, StatusProjeto? status)
        {
            var projetos = await _projetoRepository.GetByFiltroAsync(nome, status);

            var projetosDto = _mapper.Map<IEnumerable<ProjetoDto>>(projetos);

            return projetosDto;
        }

        public async Task<bool> Excluir(long id)
        {
            var projeto = await _projetoRepository.GetByIdAsync(id);

            if (projeto == null)
                throw new KeyNotFoundException("Projeto não encontrado.");

            if (projeto.Status == StatusProjeto.Iniciado ||
                projeto.Status == StatusProjeto.Planejado ||
                projeto.Status == StatusProjeto.EmAndamento ||
                projeto.Status == StatusProjeto.Encerrado)
            {
                throw new InvalidOperationException("Não é possível excluir projetos com esse status.");
            }

            await _projetoRepository.DeleteAsync(id);
            return true;
        }


        public async Task<ProjetoDto> Inserir(ProjetoNovoDto model)
        {
            var projetoEntity = _mapper.Map<Projeto>(model);

            await _projetoRepository.Inserir(projetoEntity);

            return _mapper.Map<ProjetoDto>(projetoEntity);
        }

        public async Task<bool> InserirLogin(LoginNovoDto login)
        {
            
            var usuarioEntity = new Usuario
            {
                Login = login.Usuario,
                Senha = login.Senha 
            };

            
            _context.Usuarios.Add(usuarioEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProjetoDto>> Listar()
        {
            var projetos = await _projetoRepository.Listar();
            return _mapper.Map<List<ProjetoDto>>(projetos);
        }

        public async Task<Usuario> ObterLoginPorUsuario(string usuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login == usuario);
        }

        Task IProjetoService.Alterar(long id, ProjetoEditarDto dto)
        {
            return Alterar(id, dto);
        }

        Task IProjetoService.Excluir(long id)
        {
            return Excluir(id);
        }
    }
}
