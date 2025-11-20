using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Business.Services;
using AseguradoraPTecnica.Data.Interfaces;
using AseguradoraPTecnica.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AseguradoraPTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguroController : ControllerBase
    {
        private readonly ISeguroService _seguroService;

        public SeguroController(ISeguroService seguroService)
        {
            _seguroService = seguroService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<object>> GetAll()
        {
            try
            {
                var seguros = await _seguroService.GetAllAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Se encontraron {seguros.Count()} seguro(s)",
                    data = seguros
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor: " + ex.Message
                });
            }
        }

        [HttpPost("NuevoSeguro")]
        public async Task<ActionResult<object>> NuevoSeguro([FromBody] Seguro inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Modelo inválido",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                // Mapear inputModel a entidad Seguro
                var nuevoSeguro = new Seguro
                {
                    NombreSeguro = inputModel.NombreSeguro,
                    SumaAsegurada = inputModel.SumaAsegurada,
                    Prima = inputModel.Prima,
                    EdadMinima = inputModel.EdadMinima,
                    EdadMaxima = inputModel.EdadMaxima,
                };

                var seguroCreado = await _seguroService.AddAsync(nuevoSeguro);

                return CreatedAtAction(nameof(GetById), // Método para obtener seguro por Id que debes tener
                    new { codSeguro = seguroCreado.Codigo },
                    new
                    {
                        success = true,
                        message = "Seguro creado exitosamente",
                        data = seguroCreado
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor: " + ex
                });
            }
        }

        [HttpPost("GetById")]
        public async Task<ActionResult<Seguro>> GetById(string codSeguro)
        {
            var seguro = await _seguroService.GetByIdAsync(codSeguro);
            if (seguro == null)
                return NotFound();

            return Ok(seguro);
        }

        [HttpGet("GetSegurosAsignados")]
        public async Task<ActionResult<object>> GetSegurosAsignados()
        {
            try
            {
                var seguros = await _seguroService.GetSegurosAsignadosAsync();

                return Ok(new
                {
                    success = true,
                    message = $"Se encontraron {seguros.Count()} seguro(s)",
                    data = seguros
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor: " + ex.Message
                });
            }
        }

        [HttpPost("GetSegurosAsignadosPorCedula")]
        public async Task<ActionResult<List<SeguroAsignadoDetalle>>> GetSegurosAsignadosPorCedula(string cedula)
        {
            try
            {
                var seguros = await _seguroService.GetAssignedInsurancesDetailsByCedulaAsync(cedula);

                return Ok(seguros ?? new List<SeguroAsignadoDetalle>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error interno del servidor: " + ex.Message });
            }

        }


        [HttpPost("AsignarSeguros")]
        public async Task<ActionResult<object>> AsignarSeguros([FromBody] SeguroAsignado inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Modelo inválido",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var segurosAgregados = await _seguroService.AssignInsurancesAsync(inputModel);

                return CreatedAtAction(nameof(GetSegurosAsignadosPorCedula),
                    new { cedula = inputModel.Cedula },
                    new
                    {
                        success = true,
                        message = "Seguro creado exitosamente",
                        data = segurosAgregados
                    });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor: " + ex
                });
            }
        }







    }
}
