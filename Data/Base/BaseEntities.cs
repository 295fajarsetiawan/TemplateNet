using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Base;

public class BaseEntities
{
    [Column("id")]
    public Guid Id { get; set; }   
        
    [Column("is_deleted")]
    public bool? IsDeleted { get; set; } = false;

    [Column("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [Column("deleted_at")]
    public DateTimeOffset? DeletedAt { get; set; }

    [Column("deleted_by")]
    public Guid? DeletedBy { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [Column("updated_by")]
    public Guid? UpdatedBy { get; set; }
}