namespace PublicServiceRegister.Models.Versioning;

public interface IVersionedEntity
{
    int Id { get; set; }
    int Version { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string? ModifiedBy { get; set; }
}
