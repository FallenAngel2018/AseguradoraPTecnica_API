using AseguradoraPTecnica.Business.Interfaces;
using AseguradoraPTecnica.Business.Services;
using AseguradoraPTecnica.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AseguradoraPTecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguroController : ControllerBase
    {
        private readonly ISeguroRepository _seguroService;

        public SeguroController(ISeguroRepository seguroService)
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


    }
}
