using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.Entities
{
    public class SeguroAsignado
    {
        public long IdSeguroAsignado { get; set; }
        public string? Cedula { get; set; }
        public List<string?> CodigosSeguros { get; set; }
        public int Estado { get; set; }
    }
}
