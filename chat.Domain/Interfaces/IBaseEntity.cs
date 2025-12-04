namespace chat.Domain.Interfaces;

public interface IBaseEntity
{
    DateTime? CreatedDate { get; set; }
    string? CreatedBy { get; set; }
    DateTime? UpdatedDate { get; set;}
    string? UpdatedBy { get; set; }
    DateTime? DeletedDate { get; set; }
    string? DeletedBy { get; set; }
    bool SoftDeleted { get; set; }
}