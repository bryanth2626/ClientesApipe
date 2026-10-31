using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApipe.Models
{
    [Table("proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("nombre", TypeName = "varchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "El RUC debe tener 11 dígitos.")]
        [Column("RUC", TypeName = "varchar(11)")]
        public string Ruc { get; set; } = string.Empty;

        [Required, StringLength(15)]
        [Column("telefono", TypeName = "varchar(15)")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("direccion", TypeName = "varchar(50)")]
        public string? Direccion { get; set; }
    }
}
