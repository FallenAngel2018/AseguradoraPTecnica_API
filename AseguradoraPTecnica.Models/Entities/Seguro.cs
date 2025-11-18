using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.Entities
{
    internal class Seguro
    {
        public long IdSeguro { get; set; }
        public string? Codigo { get; set; }
        public string? NombreSeguro { get; set; }
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }
        public int Estado { get; set; }
    }
}
