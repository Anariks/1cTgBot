using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database;

[Table("tbl_storages")]
public class Storage
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public string Id { get; init; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; init; }
};