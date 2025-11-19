using AseguradoraPTecnica.Models.DTOs.Seguro;
using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Business.Interfaces
{
    public interface ISeguroService
    {
        Task<IEnumerable<SeguroDTO>> GetAllAsync();
    }
}
