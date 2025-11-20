using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Data.Repositories;
using AseguradoraPTecnica.Models.DTOs.Cliente;
using AseguradoraPTecnica.Models.DTOs.Seguro;
using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Business.Services
{
    public class SeguroService : ISeguroService
    {
        private readonly ISeguroRepository _seguroRepository;

        public SeguroService(ISeguroRepository seguroRepository)
        {
            _seguroRepository = seguroRepository;
        }

        public async Task<IEnumerable<SeguroDTO>> GetAllAsync()
        {
            try
            {
                var seguros = await _seguroRepository.GetAllAsync();
                return seguros.Select(MapearSeguroDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SeguroAsignadoDetalleDTO>> GetSegurosAsignadosAsync()
        {
            try
            {
                var seguros = await _seguroRepository.GetSegurosAsignadosAsync();
                return seguros.Select(MapearSegurosAsginadosDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<SeguroDTO?> GetByIdAsync(string codSeguro)
        {
            try
            {
                var seguro = await _seguroRepository.GetByIdAsync(codSeguro);
                if (seguro == null)
                    return null;

                return MapearSeguroDTO(seguro);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<SeguroAsignadoDetalle>> GetAssignedInsurancesDetailsByCedulaAsync(string cedula)
        {
            try
            {
                var segurosAsignados = await _seguroRepository.GetAssignedInsurancesDetailsByCedulaAsync(cedula);

                return segurosAsignados == null || segurosAsignados.Count == 0 ?
                    new List<SeguroAsignadoDetalle>() :
                    segurosAsignados;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Seguro> AddAsync(Seguro seguro)
        {
            try
            {
                var seguroCreado = await _seguroRepository.AddAsync(seguro);
                return seguroCreado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<SeguroAsignadoDetalle>> AssignInsurancesAsync(SeguroAsignado seguro)
        {
            try
            {
                var seguroCreado = await _seguroRepository.AssignInsurancesAsync(seguro);
                return seguroCreado;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        private static SeguroDTO MapearSeguroDTO(Seguro seguro)
        {
            return new SeguroDTO
            {
                IdSeguro = seguro.IdSeguro,
                Codigo = seguro.Codigo,
                NombreSeguro = seguro.NombreSeguro,
                SumaAsegurada = seguro.SumaAsegurada,
                Prima = seguro.Prima,
                EdadMinima = seguro.EdadMaxima,
                EdadMaxima = seguro.EdadMaxima,
                Estado = seguro.Estado
            };
        }

        private static SeguroAsignadoDetalleDTO MapearSegurosAsginadosDTO(SeguroAsignadoDetalle seguro)
        {
            return new SeguroAsignadoDetalleDTO
            {
                IdSeguroAsignado = seguro.IdSeguroAsignado,
                Cedula = seguro.Cedula,
                CodSeguro = seguro.CodSeguro,
                Nombres = seguro.Nombres,
                NombreSeguro = seguro.NombreSeguro,
                Estado = seguro.Estado
            };
        }

    }
}
