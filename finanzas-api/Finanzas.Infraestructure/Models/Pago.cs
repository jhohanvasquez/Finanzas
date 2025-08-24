namespace Models
{
    public class Pago
    {
        public int PagoId { get; set; }
        public int DeudaId { get; set; }
        public decimal MontoPago { get; set; }
        public string MetodoPago { get; set; }
    }
}