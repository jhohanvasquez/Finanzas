﻿namespace DTOs
{
    public class RegisterUserRequest
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}