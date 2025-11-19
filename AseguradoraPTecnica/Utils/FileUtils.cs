using AseguradoraPTecnica.Models.Entities;
using ClosedXML.Excel;
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

        public static async Task<List<Cliente>> LeerClientesDesdeXlsxAsync(IFormFile archivo)
        {
            var clientes = new List<Cliente>();

            var validacionColumnas = ValidarColumnasXlsx(archivo);
            if (!validacionColumnas.esValido)
            {
                throw new Exception(validacionColumnas.mensaje);
            }

            try
            {
                using var streamOriginal = archivo.OpenReadStream();
                var memoryStream = new MemoryStream();
                await streamOriginal.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using var workbook = new XLWorkbook(memoryStream);
                var worksheet = workbook.Worksheet(1);

                if (worksheet == null)
                    return clientes;

                var firstRow = worksheet.FirstRowUsed();
                if (firstRow == null)
                    return clientes;

                var currentRow = firstRow.RowBelow();

                while (!currentRow.IsEmpty())
                {
                    var cedula = currentRow.Cell(1).GetString().Trim();
                    if (string.IsNullOrEmpty(cedula))
                    {
                        currentRow = currentRow.RowBelow();
                        continue;
                    }

                    int edad = 0;
                    int.TryParse(currentRow.Cell(5).GetString().Trim(), out edad);

                    clientes.Add(new Cliente
                    {
                        Cedula = cedula,
                        Nombres = currentRow.Cell(2).GetString().Trim(),
                        Apellidos = currentRow.Cell(3).GetString().Trim(),
                        Telefono = currentRow.Cell(4).GetString().Trim(),
                        Edad = edad,
                        Estado = 1
                    });

                    currentRow = currentRow.RowBelow();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return clientes;
        }



        public static (bool esValido, string mensaje) ValidarColumnasXlsx(IFormFile archivo)
        {
            try
            {
                using var stream = archivo.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1); // Primera hoja

                if (worksheet == null)
                    return (false, "El archivo XLSX no contiene hojas de trabajo");

                var firstRow = worksheet.FirstRowUsed();
                if (firstRow == null)
                    return (false, "La hoja de trabajo está vacía");

                var columnasEsperadas = new[] { "Cedula", "Nombres", "Apellidos", "Telefono", "Edad" };

                for (int i = 0; i < columnasEsperadas.Length; i++)
                {
                    var valorCelda = firstRow.Cell(i + 1).GetString().Trim();

                    if (string.IsNullOrEmpty(valorCelda))
                        return (false, $"La columna {i + 1} en el encabezado está vacía");

                    if (!valorCelda.Equals(columnasEsperadas[i], StringComparison.OrdinalIgnoreCase))
                        return (false, $"La columna {i + 1} debe ser '{columnasEsperadas[i]}', pero se encontró '{valorCelda}'");
                }

                return (true, "Validación de columnas exitosa");
            }
            catch (Exception ex)
            {
                return (false, $"Error al validar columnas: {ex.Message}");
            }
        }



    }
}
