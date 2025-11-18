using AseguradoraPTecnica.Models.Entities;
using AseguradoraPTecnica.Models.DTOs;

namespace AseguradoraPTecnica.Business.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> GetAllAsync();
        Task<ClienteDTO> GetByIdCedulaAsync(string cedula);
        Task<IEnumerable<ClienteDTO>> BuscarClientesAsync(
            string? cedula = null,
            string? nombresOApellidos = null,
            int? estado = null);
        Task<IEnumerable<ClienteDTO>> GetByClientAsync(Cliente cliente);
        Task<ClienteDTO> CreateClienteAsync(Cliente cliente);
        Task<long> UpdateClienteAsync(Cliente cliente);
        Task<long> DeleteClienteAsync(Cliente cliente);
    }
}
