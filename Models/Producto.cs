using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApipe.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        [Column ("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Required, Precision(7, 2)]
        [Column("precio")]
        public decimal Precio { get; set; }

        [Required]
        [Column("garantia")]
        public byte Garantia { get; set; }

        [StringLength(40)]
        [Column("numserie")]
        public string? Numserie { get; set; }
    }
}
