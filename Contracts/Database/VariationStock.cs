using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Database;

namespace Contracts.Database;

[Table("tbl_variation_stocks")]
public class VariationStock
{
    [Key]
    [MaxLength(36)]
    [Column("variation_id", Order = 0)]
    public string VariationId { get; init; }

    [Key]
    [MaxLength(36)]
    [Column("storage_id", Order = 1)]
    public string StorageId { get; init; }

    [MaxLength(100)]
    [Column("stock")]
    public float Stock { get; init; }

    public virtual Storage Storage {get; init;}
}