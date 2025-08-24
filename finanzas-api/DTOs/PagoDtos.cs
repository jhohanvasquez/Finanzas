namespace DTOs
{
    public class PagoCreateRequest
    {
        public int DeudaId { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; }
    }
}