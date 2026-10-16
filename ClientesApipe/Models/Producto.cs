using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClientesApipe.Models
{
    [Table ("productos")]
    public class Producto
    {
        [Key]
        [Column ("id")]
        public int Id { get; set; }

        [Required, StringLength(70)]
        [Column("nombre")]
        public string Nombre { get; set; } //Nombre NVARCHAR(70)  NOT NULL,
        
        
        [Required, StringLength(70)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }

        [Required]
        [Column("garantia")]
        public byte Garantia { get; set; }

        [Required, StringLength(40)]
        [Column("numserie")]
        public string Numserie { get; set; }

    }
}
