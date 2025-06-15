using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopSG.Models {

public class Product {

[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int Id { get; set; }

public string? Name { get; set; }

public string? Thumbnail { get; set; }

public string? Screen { get; set; }

public string? GraphicCard { get; set; }

public string? Connector { get; set; }

public string? OS { get; set; }

public string? Design { get; set; }

public string? Size { get; set; }

public string? Mass { get; set; }

public string? Pin { get; set; }

[ForeignKey("Category")]
public int CategoryID { get; set; }

[Column(TypeName = "decimal(10,1)")]
public decimal Rating { get; set; }

[Column(TypeName = "datetime")]
public DateTime CreatedAt { get; set; } = DateTime.Now;

[Column(TypeName = "datetime")]
public DateTime? UpdatedAt { get; set; }

// Navigation properties
public virtual Category? Category { get; set; }
public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

}

}