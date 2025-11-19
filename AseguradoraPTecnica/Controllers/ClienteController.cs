using Microsoft.AspNetCore.Mvc;
using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Models.Entities;
using AseguradoraPTecnica.Utils;

namespace AseguradoraPTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<object>> GetAll()
        {
            try
            {
                var clientes = await _clienteService.GetAllAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Se encontraron {clientes.Count()} cliente(s)",
                    data = clientes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor"
                });
            }
        }

        [HttpPost("obtener-por-cedula")]
        public async Task<ActionResult<object>> GetByCedula([FromBody] Cliente cliente)
        {
            try
            {
                var clienteEncontrado = await _clienteService.GetByIdCedulaAsync(cliente.Cedula!);

                return Ok(new
                {
                    success = true,
                    message = "Cliente encontrado",
                    data = clienteEncontrado
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"{ex.Message}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"{ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor"
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] Cliente cliente)
        {
            try
            {
                var resultado = await _clienteService.CreateClienteAsync(cliente);

                return CreatedAtAction(nameof(GetByCedula),
                    new { cedula = resultado.Cedula },
                    new
                    {
                        success = true,
                        message = "Cliente creado exitosamente",
                        data = resultado
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"{ex.Message}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = $"{ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor"
                });
            }
        }

        [HttpPost("cargar-archivo-ingreso-clientes")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<object>> IngresarMultiplesClientesPorArchivo([FromForm] IFormFile archivo)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "No se recibió ningún archivo"
                    });
                }

                var extension = Path.GetExtension(archivo.FileName).ToLower();

                if (extension != ".xlsx" && extension != ".txt")
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Solo se permiten archivos .xlsx o .txt"
                    });
                }

                if (archivo.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El archivo no debe superar los 5MB"
                    });
                }

                List<Cliente> clientes;

                if (extension == ".txt")
                {
                    clientes = await FileUtils.LeerClientesDesdeTxtAsync(archivo);
                }
                else
                {
                    clientes = await FileUtils.LeerClientesDesdeXlsx(archivo);
                }

                var resultado = await _clienteService.InsertarMultiplesClientesAsync(clientes);

                return Ok(new
                {
                    success = true,
                    message = $"Archivo '{archivo.FileName}' procesado correctamente ({archivo.Length} bytes)",
                    data = new
                    {
                        clientesInsertados = resultado.ClientesInsertados,
                        clientesConError = resultado.ClientesErrores,
                        nombreArchivo = archivo.FileName,
                        tamano = archivo.Length,
                        tipo = archivo.ContentType,
                        extension = extension
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al procesar el archivo"
                });
            }
        }







    }
}
