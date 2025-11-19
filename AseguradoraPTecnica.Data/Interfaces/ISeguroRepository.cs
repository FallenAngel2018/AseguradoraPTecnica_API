using AseguradoraPTecnica.Models.Entities;

namespace AseguradoraPTecnica.Data.Interfaces
{
    public interface ISeguroRepository
    {
        Task<IEnumerable<Seguro>> GetAllAsync();
        Task<Seguro> GetByIdAsync(string codSeguro);
        Task<Seguro> AddAsync(Seguro seguro);
    }
}
