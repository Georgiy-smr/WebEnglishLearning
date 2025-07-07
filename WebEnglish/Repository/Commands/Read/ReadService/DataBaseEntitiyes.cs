using ContextDataBase;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.Collections.Generic;
using DataBaseOperationHelper.BaseCommandsOperations.Reading;

namespace Repository.Commands.Read.ReadService;

public class DataBaseEntitiyes<T> : IEntityCollection<T> where T : Entity, new()
{
    private readonly DbSet<T> _Set;
    public DataBaseEntitiyes(AppDbContext DB)
    {
        _Set = DB.Set<T>();
    }
    public IQueryable<T> Get(ReadCommand<T> command)
    {
        IQueryable<T> query = command.Tracked ? _Set : _Set.AsNoTracking();

        if (command.Filters != null)
            foreach (var filter in command.Filters)
                query = query.Where(filter);

        if (command.Includes != null)
            foreach (var include in command.Includes)
                query = query.Include(include);

        if (command.AsSplitQuery)
            query = query.AsSplitQuery();

        query = command.OrderByDesc
            ? query.OrderByDescending(command.OrderBy)
            : query.OrderBy(command.OrderBy);

        return query.Page(command.ZeroStart, command.Size);
    }

}