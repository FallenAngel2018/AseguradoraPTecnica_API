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

                        command.Parameters.Add("@opcion", SqlDbType.Int).Value = 1;
                        command.Parameters.Add("@codigo", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        command.Parameters.Add("@nombreSeguro", SqlDbType.VarChar, 100).Value = DBNull.Value;
                        command.Parameters.Add("@sumaAsegurada", SqlDbType.Decimal).Value = DBNull.Value;
                        command.Parameters["@sumaAsegurada"].Precision = 18;
                        command.Parameters["@sumaAsegurada"].Scale = 2;

                        command.Parameters.Add("@prima", SqlDbType.Decimal).Value = DBNull.Value;
                        command.Parameters["@prima"].Precision = 18;
                        command.Parameters["@prima"].Scale = 2;

                        command.Parameters.Add("@edadMinima", SqlDbType.Int).Value = DBNull.Value;
                        command.Parameters.Add("@edadMaxima", SqlDbType.Int).Value = DBNull.Value;
                        command.Parameters.Add("@estado", SqlDbType.Int).Value = DBNull.Value;

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




        private static Seguro MapearSeguro(SqlDataReader reader)
        {
            return new Seguro
            {
                IdSeguro = (long)reader["IdSeguro"],
                Codigo = reader["Codigo"]?.ToString(),
                NombreSeguro = reader["NombreSeguro"]?.ToString(),
                SumaAsegurada = reader["SumaAsegurada"] != DBNull.Value ? (decimal)reader["SumaAsegurada"] : 0m,
                Prima = reader["Prima"] != DBNull.Value ? (decimal)reader["Prima"] : 0m,
                EdadMinima = reader["EdadMinima"] != DBNull.Value ? (int)reader["EdadMinima"] : 0,
                EdadMaxima = reader["EdadMaxima"] != DBNull.Value ? (int)reader["EdadMaxima"] : 0,
                Estado = (int)reader["Estado"]
            };
        }

    }
}
