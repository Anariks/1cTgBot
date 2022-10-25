using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Contracts.Database;

namespace Contracts.Database;

[Table("tbl_variations")]
public class Variation
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public string Id { get; init; }

    [Required]
    [MaxLength(500)]
    [Column("name")]
    public string Name { get; init; }

    [ForeignKey(nameof(Product))]
    [MaxLength(36)]
    [Column("product_id")]
    public string ProductId { get; init; }

    [Column("quantity")]
    public float Quantity { get; init; }
    //public virtual Product Product { get; init; }

    public virtual List<VariationPrice> VariationPrices { get; init; }
    public virtual List<VariationStock> VariationStocks { get; init; }
}