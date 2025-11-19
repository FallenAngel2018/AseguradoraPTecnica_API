using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.DTOs.Cliente
{
    public class ResultadoIngresoClientesDTO
    {
        public List<ClienteDTO> ClientesInsertados { get; set; } = new List<ClienteDTO>();
        public List<ClienteErrorDTO> ClientesErrores { get; set; } = new List<ClienteErrorDTO>();
    }
}
