using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contracts.Database;

[Table("tbl_users")]
public class User
{
    [Key]
    [MaxLength(36)]
    [Column("id")]
    public int Id { get; init; }

    [ForeignKey(nameof(UserRoleId))]
    [Column("user_role_id")]
    public int UserRoleId { get; init; }

    public virtual UserRole? UserRole { get; init; }
};