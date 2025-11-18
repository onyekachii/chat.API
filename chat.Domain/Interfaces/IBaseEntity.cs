namespace chat.Domain.Interfaces;

public interface IBaseEntity
{
    DateTimeOffset? CreatedDate { get; set; }
    long? CreatedBy { get; set; }
    DateTimeOffset? UpdatedDate { get; set;}
    long? UpdatedBy { get; set; }
    DateTimeOffset? DeletedDate { get; set; }
    long? DeletedBy { get; set; }
    bool SoftDeleted { get; set; }
}