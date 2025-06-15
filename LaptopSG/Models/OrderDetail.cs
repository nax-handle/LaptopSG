using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSG.Models
{
    public class OrderDetail
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("ProductVariant")]
        public int ProductId { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int Total { get; set; }

        [Column(TypeName = "date")]
        public DateTime Deadline { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Order? Order { get; set; }
        public virtual ProductVariant? ProductVariant { get; set; }
    }
} 