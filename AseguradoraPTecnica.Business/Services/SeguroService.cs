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
                return seguros.Select(MapearDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private static SeguroDTO MapearDTO(Seguro seguro)
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
    }
}
