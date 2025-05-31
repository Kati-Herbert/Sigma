using System.ComponentModel.DataAnnotations;

namespace Sigma.Domain.Enums
{
    public enum StatusProjeto
    {
        [Display(Name = "Em Análise")]
        EmAnalise,

        [Display(Name = "Análise Realizada")]
        AnaliseRealizada,

        [Display(Name = "Análise Aprovada")]
        AnaliseAprovada,

        [Display(Name = "Iniciado")]
        Iniciado,

        [Display(Name = "Planejado")]
        Planejado,

        [Display(Name = "Em Andamento")]
        EmAndamento,

        [Display(Name = "Encerrado")]
        Encerrado,

        [Display(Name = "Cancelado")]
        Cancelado
    }
}