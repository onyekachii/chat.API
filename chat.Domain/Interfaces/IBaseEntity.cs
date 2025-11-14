namespace chat.Domain.Interfaces;

interface IBaseEntity<T>
{
    DateTimeOffset CreatedDate { get; set; }
    T CreatedBy { get; set; }
    DateTimeOffset UpdatedDate { get; set;}
    T UpdatedBy { get; set; }
    DateTimeOffset DeletedDate { get; set; }
    T DeletedBy { get; set; }
    Bool SoftDeleted { get; set; }
}