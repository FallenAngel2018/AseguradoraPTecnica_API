using Microsoft.AspNetCore.Mvc;
using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Models.Entities;

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

        [HttpGet]
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


        



    }
}
