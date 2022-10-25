using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Contracts.Database;

namespace Contracts.Database;

[Table("tbl_variation_prices")]
public class VariationPrice
{
    [Key]
    [MaxLength(36)]
    [Column("variation_id", Order = 0)]
    public string VariationId { get; init; }

    [Key]
    [MaxLength(36)]
    [Column("price_type_id", Order = 1)]
    public string PriceTypeId { get; init; }

    [Column("amount")]
    public float Price { get; init; }
    public virtual PriceType PriceType {get; init;}
}