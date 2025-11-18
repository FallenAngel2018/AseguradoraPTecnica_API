using System.Data;
using Microsoft.Data.SqlClient;
using AseguradoraPTecnica.Data.Context;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DatabaseConnection _databaseConnection;
        private const string SP_CLIENTE_CONSULTAR = "apt_cliente_consultas";
        private const string SP_CLIENTE_GESTION = "apt_cliente_gestion";
        private const int SqlCommandTimeout_Segs = 30;

        public ClienteRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }


        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var clientes = new List<Cliente>();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("apt_cliente_consultas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        command.Parameters.Add("@nombres_o_apellidos", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = DBNull.Value;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientes.Add(MapearCliente(reader));
                            }
                        }
                    }
                }

                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cliente> GetByIdCedulaAsync(string cedula)
        {
            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("apt_cliente_consultas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cedula;
                        command.Parameters.Add("@nombres_o_apellidos", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = DBNull.Value;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapearCliente(reader);
                            }
                        }
                    }
                }

                return new Cliente();
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Cliente>> BuscarClientesAsync(
            string? cedula = null,
            string? nombresOApellidos = null,
            int? estado = null)
        {
            var clientes = new List<Cliente>();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("apt_cliente_consultas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cedula;
                        command.Parameters.Add("@nombres_o_apellidos", SqlDbType.VarChar, 100).Value = nombresOApellidos;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = estado;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientes.Add(MapearCliente(reader));
                            }
                        }
                    }
                }

                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<Cliente>> GetByClientAsync(Cliente cliente)
        {
            var clientes = new List<Cliente>();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("apt_cliente_consultas", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30;

                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cliente.Cedula;
                        command.Parameters.Add("@nombres_o_apellidos", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = DBNull.Value;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientes.Add(MapearCliente(reader));
                            }
                        }
                    }
                }

                return clientes;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static Cliente MapearCliente(SqlDataReader reader)
        {
            return new Cliente
            {
                IdAsegurado = (long)reader["IdAsegurado"],
                Cedula = reader["Cedula"]?.ToString(),
                Nombres = reader["Nombres"]?.ToString(),
                Apellidos = reader["Apellidos"]?.ToString(),
                Telefono = reader["Telefono"] != DBNull.Value ? reader["Telefono"]?.ToString() : null,
                Edad = reader["Edad"] != DBNull.Value ? (int)reader["Edad"] : 0,
                Estado = (int)reader["Estado"]
            };
        }

        public async Task<long> AddAsync(Cliente cliente)
        {
            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_CLIENTE_GESTION, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@opcion", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cliente.Cedula;
                        command.Parameters.Add("@nombres", SqlDbType.VarChar, 100).Value = cliente.Nombres;
                        command.Parameters.Add("@apellidos", SqlDbType.VarChar, 100).Value = cliente.Apellidos;
                        command.Parameters.Add("@telefono", SqlDbType.VarChar, 50).Value = cliente.Telefono;
                        command.Parameters.Add("@edad", SqlDbType.Int).Value = cliente.Edad;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = cliente.Estado;

                        var result = await command.ExecuteScalarAsync();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new InvalidOperationException("El SP no devolvió el ID del cliente.");
                        }

                        long clienteId = Convert.ToInt64(result);

                        return clienteId;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new InvalidOperationException(
                    $"Cliente duplicado - Cédula: {cliente.Cedula}", ex);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<long> UpdateAsync(Cliente cliente)
        {
            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_CLIENTE_GESTION, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@opcion", SqlDbType.Int).Value = 2;
                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cliente.Cedula;
                        command.Parameters.Add("@nombres", SqlDbType.VarChar, 100).Value = cliente.Nombres;
                        command.Parameters.Add("@apellidos", SqlDbType.VarChar, 100).Value = cliente.Apellidos;
                        command.Parameters.Add("@telefono", SqlDbType.VarChar, 50).Value = cliente.Telefono;
                        command.Parameters.Add("@edad", SqlDbType.Int).Value = cliente.Edad;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = cliente.Estado;

                        var result = await command.ExecuteScalarAsync();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new InvalidOperationException("El SP no devolvió el ID del cliente actualizado.");
                        }

                        long clienteId = Convert.ToInt64(result);

                        return clienteId;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> DeleteAsync(Cliente cliente)
        {
            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_CLIENTE_GESTION, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@opcion", SqlDbType.Int).Value = 3;
                        command.Parameters.Add("@cedula", SqlDbType.VarChar, 50).Value = cliente.Cedula;

                        var result = await command.ExecuteScalarAsync();

                        if (result == null || result == DBNull.Value)
                        {
                            throw new InvalidOperationException("El SP no devolvió el ID del cliente eliminado.");
                        }

                        long clienteId = Convert.ToInt64(result);

                        return clienteId;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error en la base de datos: {ex.Message}", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

