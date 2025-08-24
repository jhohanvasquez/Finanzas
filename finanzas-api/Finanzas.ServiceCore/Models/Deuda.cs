namespace Models
{
    public class Deuda
    {
        public int DeudaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal MontoTotal { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}