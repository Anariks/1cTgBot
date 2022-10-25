using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database;

[Table("tbl_price_types")]
public class PriceType
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public string Id {get; init;}
    
    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name {get; init;}

    [Required]
    [MaxLength(3)]
    [Column("currency_code")]
    public string CurrencyCode {get; init;}
};