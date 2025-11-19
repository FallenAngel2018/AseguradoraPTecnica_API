using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.DTOs.Cliente
{
    public class ClienteErrorDTO
    {
        public string Cedula { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Edad { get; set; }
        public string ErrorMensaje { get; set; }
    }
}
