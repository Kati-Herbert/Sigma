using Microsoft.EntityFrameworkCore;
using Sigma.Domain.Entities;
using Sigma.Domain.Enums;
using Sigma.Domain.Interfaces.Repositories;
using Sigma.Infra.Data.Context;

namespace Sigma.Infra.Data.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly SigmaContext _dbContext;

        public ProjetoRepository(SigmaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAsync(long id)
        {
            var projeto = await _dbContext.Projetos.FindAsync(id);

            _dbContext.Projetos.Remove(projeto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Projeto>> GetByFiltroAsync(string? nome, StatusProjeto? status)
        {
            var query = _dbContext.Projetos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.Nome.Contains(nome));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Projeto> GetByIdAsync(long id)
        {
            return await _dbContext.Projetos.FindAsync(id);
        }

        public async Task<Projeto> Inserir(Projeto entidade)
        {
            if (entidade.Status == StatusProjeto.Encerrado)
                entidade.DataRealTermino = DateTime.UtcNow;

            await _dbContext.Set<Projeto>().AddAsync(entidade);
            await _dbContext.SaveChangesAsync();
            return entidade; 
        }

        public async Task<List<Projeto>> Listar()
        {
            return await _dbContext.Set<Projeto>().ToListAsync();
        }
        public async Task UpdateAsync(Projeto projeto)
        {
            _dbContext.Projetos.Update(projeto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
