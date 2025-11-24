namespace chat.Domain.Interfaces;

public interface IBaseEntity
{
    DateTimeOffset? CreatedDate { get; set; }
    string? CreatedBy { get; set; }
    DateTimeOffset? UpdatedDate { get; set;}
    string? UpdatedBy { get; set; }
    DateTimeOffset? DeletedDate { get; set; }
    string? DeletedBy { get; set; }
    bool SoftDeleted { get; set; }
}