using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database;

[Table("tbl_categories")]
public class Category
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public string? Id { get; init; }

    [Required]
    [MaxLength(300)]
    [Column("name")]
    public string? Name { get; init; }

    [MaxLength(36)]
    [Column("parent_category_id")]
    public string? ParentCategoryId { get; init; }

    [ForeignKey(nameof(ParentCategoryId))]
    public virtual Category? PartentCategory { get; init; }
}