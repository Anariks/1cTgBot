using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database;

[Table("tbl_user_roles")]
public class UserRole
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; init; }

    [MaxLength(1000)]
    [Column("price_types_id")]
    public List<string>? PriceTypesId { get; init; }

    [MaxLength(1000)]
    [Column("storages_id")]
    public List<string>? StoragesId { get; init; }

    [Column("is_admin")]
    public bool IsAdmin { get; init; }

    public virtual List<PriceType>? PriceTypes { get; init; }
    public virtual List<Storage>? Storages { get; init; }
};