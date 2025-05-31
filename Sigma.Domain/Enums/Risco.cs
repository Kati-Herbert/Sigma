using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.Domain.Enums
{
    public enum Risco
    {
        [Display(Name = "Baixo")]
        Baixo,
        [Display(Name = "Medio")]
        Medio,
        [Display(Name = "Alto")]
        Alto
    }
}
