using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Contracts.Database;

namespace Contracts.Database;

[Table("tbl_products")]
public class Product
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public string Id { get; init; }

    [Required]
    [MaxLength(500)]
    [Column("name")]
    public string Name { get; init; }

    [MaxLength(100)]
    [Column("sku")]
    public string Sku { get; init; }

    [ForeignKey(nameof(Category))]
    [MaxLength(200)]
    [Column("category_id")]
    public string CategoryId { get; init; }

    [MaxLength(200)]
    [Column("brand_id")]
    public string BrandId { get; init; }

    [MaxLength(300)]
    [Column("url")]
    public string Url { get; set; }

    public virtual Category Category { get; init; }
    [ForeignKey(nameof(BrandId))]
    public virtual Brand Brand { get; init; }

    public virtual IEnumerable<Variation> Variations { get; set; }
}