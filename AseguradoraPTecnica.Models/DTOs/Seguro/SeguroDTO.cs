using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.DTOs.Seguro
{
    public class SeguroDTO
    {
        public long IdSeguro { get; set; }
        public string? Codigo { get; set; }
        public string? NombreSeguro { get; set; }
        public decimal? SumaAsegurada { get; set; }
        public decimal? Prima { get; set; }
        public int EdadMinima { get; set; }
        public int EdadMaxima { get; set; }
        public int Estado { get; set; }
    }
}
