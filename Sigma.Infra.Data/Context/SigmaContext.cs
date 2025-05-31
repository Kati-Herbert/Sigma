using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Sigma.Domain.Entities;

namespace Sigma.Infra.Data.Context
{
    public class SigmaContext : DbContext
    {
        public SigmaContext(DbContextOptions<SigmaContext> options) : base(options) { }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName((entity.GetTableName() ?? string.Empty).ToLower());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName((property.Name ?? string.Empty).ToLower());
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName((key.GetName() ?? string.Empty).ToLower());
                }

                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.SetConstraintName((foreignKey.GetConstraintName() ?? string.Empty).ToLower());
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName((index.GetDatabaseName() ?? string.Empty).ToLower());
                }
            }
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
