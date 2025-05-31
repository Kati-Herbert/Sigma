using Sigma.Domain.Enums;

namespace Sigma.Domain.Dtos
{
    public class ProjetoNovoDto
    {

        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? PrevisaoTermino { get; set; }
        public decimal? OrcamentoTotal { get; set; }
        public Risco? Risco { get; set; }
        public StatusProjeto? Status { get; set; }

    }
}
