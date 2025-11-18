namespace chat.Domain.Interfaces;

public interface IBaseEntity
{
    DateTimeOffset? CreatedDate { get; set; }
    Guid? CreatedBy { get; set; }
    DateTimeOffset? UpdatedDate { get; set;}
    Guid? UpdatedBy { get; set; }
    DateTimeOffset? DeletedDate { get; set; }
    Guid? DeletedBy { get; set; }
    bool SoftDeleted { get; set; }
}