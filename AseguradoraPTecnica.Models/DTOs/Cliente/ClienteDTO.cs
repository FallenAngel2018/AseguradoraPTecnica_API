namespace AseguradoraPTecnica.Models.DTOs.Cliente
{
    public class ClienteDTO
    {
        public long IdAsegurado { get; set; }
        public string? Cedula { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Telefono { get; set; }
        public int Edad { get; set; }
        public int Estado { get; set; }
    }
}
