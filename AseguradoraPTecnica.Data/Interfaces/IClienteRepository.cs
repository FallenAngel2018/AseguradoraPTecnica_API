using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Data.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdCedulaAsync(string cedula);
        Task<IEnumerable<Cliente>> BuscarClientesAsync(
            string? cedula = null,
            string? nombresOApellidos = null,
            int? estado = null);
        Task<IEnumerable<Cliente>> GetByClientAsync(Cliente cliente);
        Task<long> AddAsync(Cliente cliente);
        Task<long> UpdateAsync(Cliente cliente);
        Task<long> DeleteAsync(Cliente cliente);
    }
}
