namespace Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; } = 0;
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}