using AseguradoraPTecnica.Data.Context;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Models.DTOs.Cliente;
using AseguradoraPTecnica.Models.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

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

                    using (var command = new SqlCommand(SP_CLIENTE_CONSULTAR, connection))
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

                    using (var command = new SqlCommand(SP_CLIENTE_CONSULTAR, connection))
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

        public async Task<ResultadoIngresoClientesDTO> InsertarMultiplesClientesAsync(List<Cliente> clientes)
        {
            var resultado = new ResultadoIngresoClientesDTO
            {
                ClientesInsertados = new List<ClienteDTO>(),
                ClientesErrores = new List<ClienteErrorDTO>()
            };

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("apt_cliente_ingreso_masivo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        // Crear DataTable para el TVP
                        var table = new DataTable();
                        table.Columns.Add("Cedula", typeof(string));
                        table.Columns.Add("Nombres", typeof(string));
                        table.Columns.Add("Apellidos", typeof(string));
                        table.Columns.Add("Telefono", typeof(string));
                        table.Columns.Add("Edad", typeof(string));

                        foreach (var c in clientes)
                        {
                            table.Rows.Add(
                                c.Cedula,
                                c.Nombres,
                                c.Apellidos,
                                c.Telefono,
                                c.Edad.ToString()
                            );
                        }

                        var tvpParam = command.Parameters.AddWithValue("@Clientes", table);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.ClienteTableType";

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Primer resultado: clientes insertados bien
                            while (await reader.ReadAsync())
                            {
                                resultado.ClientesInsertados.Add(new ClienteDTO
                                {
                                    IdAsegurado = reader.GetInt64(0),
                                    Cedula = reader.GetString(1),
                                    Nombres = reader.GetString(2),
                                    Apellidos = reader.GetString(3),
                                    Telefono = reader.GetString(4),
                                    Edad = reader.GetInt32(5),
                                    Estado = reader.GetInt32(6),
                                });
                            }

                            if (await reader.NextResultAsync())
                            {
                                if (reader.HasRows)
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        resultado.ClientesErrores.Add(new ClienteErrorDTO
                                        {
                                            Cedula = reader.IsDBNull(0) ? null : reader.GetString(0),
                                            Nombres = reader.IsDBNull(1) ? null : reader.GetString(1),
                                            Apellidos = reader.IsDBNull(2) ? null : reader.GetString(2),
                                            Telefono = reader.IsDBNull(3) ? null : reader.GetString(3),

                                            Edad = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString(),

                                            ErrorMensaje = "Cliente duplicado" // Como tú tienes solo ese caso en SP
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en InsertarMultiplesClientesAsync: {ex.Message}", ex);
            }

            return resultado;
        }



    }
}

