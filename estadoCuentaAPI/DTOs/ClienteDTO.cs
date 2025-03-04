namespace estadoCuentaAPI.DTOs
{
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;

        public string Dui { get; set; } = null!;

    }
}
