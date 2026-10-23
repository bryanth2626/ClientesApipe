using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ClientesApipe.Models
{   
    //Remplazamops la clase AuthDtos que viene por defecto
    public class RegistroRequest
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
