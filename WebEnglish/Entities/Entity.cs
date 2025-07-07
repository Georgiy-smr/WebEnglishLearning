
using DataBaseOperationHelper.Abstractions;

namespace Entities;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
    public bool SoftDeleted { get; set; }
}