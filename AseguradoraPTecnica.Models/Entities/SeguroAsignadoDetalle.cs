using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.Entities
{
    public class SeguroAsignadoDetalle
    {
        public long IdSeguroAsignado { get; set; }
        public string? Cedula { get; set; }
        public string? Nombres { get; set; }
        public string? CodSeguro { get; set; }
        public string? NombreSeguro { get; set; }
        public int Estado { get; set; }
    }

}
