using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClientesApipe.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required, StringLength(50)]
        [Column("username")]
        public string Username { get; set; }
        [Required, StringLength(200)]
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Required, StringLength(30)]
        [Column("rol")]
        public string Rol {  get; set; }
    }
}
