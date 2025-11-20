using AseguradoraPTecnica.Models.DTOs.Seguro;
using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Business.Interfaces
{
    public interface ISeguroService
    {
        Task<IEnumerable<SeguroDTO>> GetAllAsync();
        Task<IEnumerable<SeguroAsignadoDetalleDTO>> GetSegurosAsignadosAsync();
        Task<SeguroDTO?> GetByIdAsync(string codSeguro);
        Task<List<SeguroAsignadoDetalle>> GetAssignedInsurancesDetailsByCedulaAsync(string cedula);
        Task<Seguro> AddAsync(Seguro seguro);
        Task<List<SeguroAsignadoDetalle>> AssignInsurancesAsync(SeguroAsignado seguro);
        

    }

    
}
