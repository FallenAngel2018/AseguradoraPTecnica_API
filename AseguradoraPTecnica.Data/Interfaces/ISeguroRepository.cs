using AseguradoraPTecnica.Models.DTOs.Seguro;
using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Data.Interfaces
{
    public interface ISeguroRepository
    {
        Task<IEnumerable<Seguro>> GetAllAsync();
        Task<List<SeguroAsignadoDetalle>> GetSegurosAsignadosAsync();
        Task<Seguro> GetByIdAsync(string codSeguro);
        Task<List<AssignedInsuranceOrClientDto>> GetAssignedInsurancesOrClientsAsync(BusquedaSeguroRequest busqueda);
        Task<List<SeguroAsignadoDetalle>> GetAssignedInsurancesDetailsByCedulaAsync(string cedula);
        Task<Seguro> AddAsync(Seguro seguro);
        Task<List<SeguroAsignadoDetalle>> AssignInsurancesAsync(SeguroAsignado seguro);

    }
}
