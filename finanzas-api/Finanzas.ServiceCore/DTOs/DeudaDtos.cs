namespace DTOs
{
    public class DeudaCreateRequest
    {
        public int UsuarioId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }

    public class DeudaEditRequest
    {
        public int DeudaId { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }
}