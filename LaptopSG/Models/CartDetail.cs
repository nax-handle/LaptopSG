using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSG.Models
{
    public class CartDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProductVariant")]
        public int ProductId { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        public int Quantity { get; set; } = 1;

        public int Total { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ProductVariant? ProductVariant { get; set; }
        public virtual Cart? Cart { get; set; }
    }
} 