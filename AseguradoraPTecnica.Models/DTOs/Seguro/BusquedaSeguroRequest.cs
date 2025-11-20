using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Models.DTOs.Seguro
{
    public class BusquedaSeguroRequest
    {
        public string Busqueda { get; set; } = string.Empty;

        /// <summary>
        /// Opción de búsqueda: 1 = Por cédula, 2 = Por código de seguro
        /// </summary>
        public int Opcion { get; set; }
    }
}
