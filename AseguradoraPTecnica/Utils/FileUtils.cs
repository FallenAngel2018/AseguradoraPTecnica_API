using AseguradoraPTecnica.Models.Entities;
using OfficeOpenXml;

namespace AseguradoraPTecnica.Utils
{
    public class FileUtils
    {
        public static async Task<List<Cliente>> LeerClientesDesdeTxtAsync(IFormFile archivo)
        {
            using var stream = archivo.OpenReadStream();
            stream.Position = 0;

            var clientes = new List<Cliente>();

            using (var reader = new StreamReader(stream, leaveOpen: true))
            {
                var contenido = await reader.ReadToEndAsync();
                var lineas = contenido.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                // Saltar encabezado (línea 0)
                for (int i = 1; i < lineas.Length; i++)
                {
                    var campos = lineas[i].Split('|');

                    var cliente = new Cliente
                    {
                        Cedula = campos[0].Trim(),
                        Nombres = campos[1].Trim(),
                        Apellidos = campos[2].Trim(),
                        Telefono = campos[3].Trim(),
                        Edad = int.Parse(campos[4].Trim())
                    };

                    clientes.Add(cliente);
                }
            }

            return clientes;
        }


        public static async Task<List<Cliente>> LeerClientesDesdeXlsx(IFormFile archivo)
        {
            using var stream = archivo.OpenReadStream();
            stream.Position = 0;

            var clientes = new List<Cliente>();

            using (var package = new ExcelPackage())
            {
                await package.LoadAsync(stream);
                var worksheet = package.Workbook.Worksheets[0];

                var filaInicio = worksheet.Dimension.Start.Row;
                var totalFilas = worksheet.Dimension.End.Row;

                // Saltar encabezado
                for (int fila = filaInicio + 1; fila <= totalFilas; fila++)
                {
                    var cliente = new Cliente
                    {
                        Cedula = worksheet.Cells[fila, 1].Value?.ToString()?.Trim(),
                        Nombres = worksheet.Cells[fila, 2].Value?.ToString()?.Trim(),
                        Apellidos = worksheet.Cells[fila, 3].Value?.ToString()?.Trim(),
                        Telefono = worksheet.Cells[fila, 4].Value?.ToString()?.Trim(),
                        Edad = int.Parse(worksheet.Cells[fila, 5].Value?.ToString()?.Trim())
                    };

                    clientes.Add(cliente);
                }
            }

            return clientes;
        }




    }
}
