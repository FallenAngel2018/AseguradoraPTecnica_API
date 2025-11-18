using System.Data;
using Microsoft.Data.SqlClient;

namespace AseguradoraPTecnica.Data.Context
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task TestConnectionAsync()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();
                    if (connection.State == ConnectionState.Open)
                    {
                        Console.WriteLine("Conexión a BD exitosa");
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar: {ex.Message}");
                throw;
            }
        }
    }
}

