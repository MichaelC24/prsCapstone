using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace prsCapstone.Model
{
    [Index(nameof(PartNbr), IsUnique = true)]
    public class Product
    {

        public int Id { get; set; }

        [StringLength(30)]
        public string PartNbr { get; set; } = string.Empty;

        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(11,2)")]
        public decimal Price { get; set; }

        [StringLength(30)]
        public string Unit { get; set; } = string.Empty;

        [StringLength(255)]
        public string? PhotoPath { get; set; } = string.Empty;

        public int VendorId { get; set; }

        public virtual Vendor? Vendor { get; set; }
    }
}
