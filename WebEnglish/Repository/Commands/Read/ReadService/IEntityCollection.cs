using Entities;
using System.Collections.Generic;

namespace Repository.Commands.Read.ReadService;


public interface IEntityCollection<T> where T : IEntity, new()
{
    IQueryable<T> Get(
        ReadCommand<T> command);
}