using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApipe.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]

        public int Id { get; set; }

        [Required, StringLength(70)]
        [Column("apellidos")]

        public string Apellidos { get; set; } = string.Empty; // apeellidos  NVARCHAR (70) NOT NULL,


        [Required, StringLength(70)]
        [Column("nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Required, StringLength(9)]
        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;


        [Required, StringLength(70)]
        [Column("correo")]
        public string Correo {  get; set; } = string.Empty;

        [Required, StringLength(8)]
        [Column("dni")]
        public string Dni {  get; set; } = string.Empty;

        [Required, StringLength(150)]
        [Column("direccion")]
        public string Direccion {  get; set; } = string.Empty;
    }
}


