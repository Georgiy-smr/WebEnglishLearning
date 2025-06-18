using ContextDataBase;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository.Commands.Read.ReadService;

public class DataBaseEntitiyes<T> : IEntityCollection<T> where T : Entity, new()
{
    private readonly DbSet<T> _Set;
    public DataBaseEntitiyes(AppDbContext DB)
    {
        _Set = DB.Set<T>();
    }
    public IQueryable<T> Get(ReadCommand<T> command, bool tracked = true)
    {
        var query = tracked ? _Set.AsQueryable() : _Set.AsQueryable().AsNoTracking();
        return query
            .ApplyFilters(command.Filters)
            .ApplyInclude(command.Includes)
            .OrderByDesc()
            .Page(command.ZeroStart, command.Size);
    }
}