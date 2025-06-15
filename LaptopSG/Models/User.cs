using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSG.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? Phone { get; set; }

        [Column(TypeName = "int")]
        public int Point { get; set; } = 0;

        public string? Gender { get; set; }

        public string? FullName { get; set; }

        public string? PasswordHash { get; set; }

        public string? Address { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
} 