using AseguradoraPTecnica.Data.Context;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Models.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AseguradoraPTecnica.Data.Repositories
{
    public class SeguroRepository : ISeguroRepository
    {

        private readonly DatabaseConnection _databaseConnection;
        private const string SP_SEGURO_CONSULTAR = "apt_seguro_consultas";
        private const string SP_SEGURO_GESTION = "apt_seguro_gestion";
        private const int SqlCommandTimeout_Segs = 30;

        public SeguroRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Seguro>> GetAllAsync()
        {
            var seguros = new List<Seguro>();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_SEGURO_CONSULTAR, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 50).Value = DBNull.Value;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                seguros.Add(MapearSeguro(reader));
                            }
                        }
                    }
                }

                return seguros;
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


        public async Task<Seguro> GetByIdAsync(string codSeguro)
        {
            var seguro = new Seguro();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_SEGURO_CONSULTAR, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 50).Value = codSeguro;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                seguro = MapearSeguro(reader);
                            }
                        }
                    }
                }

                return seguro;
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

        public async Task<Seguro> AddAsync(Seguro seguro)
        {
            var seguroNuevo = new Seguro();

            try
            {
                using (var connection = _databaseConnection.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(SP_SEGURO_GESTION, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = SqlCommandTimeout_Segs;

                        command.Parameters.Add("@opcion", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 50).Value = seguro.Codigo;
                        command.Parameters.Add("@nombreSeguro", SqlDbType.VarChar, 100).Value = seguro.NombreSeguro;
                        command.Parameters.Add("@sumaAsegurada", SqlDbType.Decimal).Value = seguro.SumaAsegurada;
                        command.Parameters["@sumaAsegurada"].Precision = 18;
                        command.Parameters["@sumaAsegurada"].Scale = 2;

                        command.Parameters.Add("@prima", SqlDbType.Decimal).Value = seguro.Prima;
                        command.Parameters["@prima"].Precision = 18;
                        command.Parameters["@prima"].Scale = 2;

                        command.Parameters.Add("@edadMinima", SqlDbType.Int).Value = seguro.EdadMinima;
                        command.Parameters.Add("@edadMaxima", SqlDbType.Int).Value = seguro.EdadMaxima;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = seguro.Estado;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                seguroNuevo = MapearSeguro(reader);
                            }
                        }
                    }
                }

                return seguroNuevo;
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



        private static Seguro MapearSeguro(SqlDataReader reader)
        {
            var seguro = new Seguro();

            if (reader.HasColumn("IdSeguro") && !reader.IsDBNull(reader.GetOrdinal("IdSeguro")))
                seguro.IdSeguro = reader.GetInt64(reader.GetOrdinal("IdSeguro"));

            if (reader.HasColumn("Codigo") && !reader.IsDBNull(reader.GetOrdinal("Codigo")))
                seguro.Codigo = reader.GetString(reader.GetOrdinal("Codigo"));

            if (reader.HasColumn("NombreSeguro") && !reader.IsDBNull(reader.GetOrdinal("NombreSeguro")))
                seguro.NombreSeguro = reader.GetString(reader.GetOrdinal("NombreSeguro"));

            if (reader.HasColumn("SumaAsegurada") && !reader.IsDBNull(reader.GetOrdinal("SumaAsegurada")))
                seguro.SumaAsegurada = reader.GetDecimal(reader.GetOrdinal("SumaAsegurada"));

            if (reader.HasColumn("Prima") && !reader.IsDBNull(reader.GetOrdinal("Prima")))
                seguro.Prima = reader.GetDecimal(reader.GetOrdinal("Prima"));

            if (reader.HasColumn("EdadMinima") && !reader.IsDBNull(reader.GetOrdinal("EdadMinima")))
                seguro.EdadMinima = reader.GetInt32(reader.GetOrdinal("EdadMinima"));

            if (reader.HasColumn("EdadMaxima") && !reader.IsDBNull(reader.GetOrdinal("EdadMaxima")))
                seguro.EdadMaxima = reader.GetInt32(reader.GetOrdinal("EdadMaxima"));

            if (reader.HasColumn("Estado") && !reader.IsDBNull(reader.GetOrdinal("Estado")))
                seguro.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));

            return seguro;
        }


    }

    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
