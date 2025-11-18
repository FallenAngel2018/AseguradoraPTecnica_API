using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Models.Entities;
using AseguradoraPTecnica.Models.DTOs;

namespace AseguradoraPTecnica.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();
                return clientes.Select(MapearDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ClienteDTO> GetByIdCedulaAsync(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula))
                    throw new ArgumentException("La cédula es requerida");

                var cliente = await _clienteRepository.GetByIdCedulaAsync(cedula);

                if (cliente == null)
                    throw new InvalidOperationException("Cliente no encontrado");

                return MapearDTO(cliente);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDTO>> BuscarClientesAsync(
            string? cedula = null,
            string? nombresOApellidos = null,
            int? estado = null)
        {
            try
            {
                var clientes = await _clienteRepository.BuscarClientesAsync(cedula, nombresOApellidos, estado);
                return clientes.Select(MapearDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDTO>> GetByClientAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    throw new ArgumentException("El cliente no puede ser nulo");

                var clientes = await _clienteRepository.GetByClientAsync(cliente);
                return clientes.Select(MapearDTO).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ClienteDTO> CreateClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    throw new ArgumentException("El cliente no puede ser nulo");

                if (string.IsNullOrWhiteSpace(cliente.Cedula))
                    throw new ArgumentException("La cédula es requerida");

                if (string.IsNullOrWhiteSpace(cliente.Nombres))
                    throw new ArgumentException("El nombre es requerido");

                if (string.IsNullOrWhiteSpace(cliente.Apellidos))
                    throw new ArgumentException("El apellido es requerido");

                // Asignar valores por defecto
                cliente.Estado = 1;

                // Llamar al repositorio
                long id = await _clienteRepository.AddAsync(cliente);

                // Devolver DTO con el ID generado
                return new ClienteDTO
                {
                    IdAsegurado = id,
                    Cedula = cliente.Cedula,
                    Nombres = cliente.Nombres,
                    Apellidos = cliente.Apellidos,
                    Telefono = cliente.Telefono,
                    Edad = cliente.Edad,
                    Estado = cliente.Estado
                };
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> UpdateClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    throw new ArgumentException("El cliente no puede ser nulo");

                if (cliente.IdAsegurado <= 0)
                    throw new ArgumentException("El ID del cliente es requerido");

                if (string.IsNullOrWhiteSpace(cliente.Cedula))
                    throw new ArgumentException("La cédula es requerida");

                if (string.IsNullOrWhiteSpace(cliente.Nombres))
                    throw new ArgumentException("El nombre es requerido");

                if (string.IsNullOrWhiteSpace(cliente.Apellidos))
                    throw new ArgumentException("El apellido es requerido");

                return await _clienteRepository.UpdateAsync(cliente);
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // ✅ ELIMINAR CLIENTE
        public async Task<long> DeleteClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                    throw new ArgumentException("El cliente no puede ser nulo");

                if (cliente.IdAsegurado <= 0)
                    throw new ArgumentException("El ID del cliente es requerido");

                return await _clienteRepository.DeleteAsync(cliente);
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static ClienteDTO MapearDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                IdAsegurado = cliente.IdAsegurado,
                Cedula = cliente.Cedula,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Telefono = cliente.Telefono,
                Edad = cliente.Edad,
                Estado = cliente.Estado
            };
        }
    }
}
