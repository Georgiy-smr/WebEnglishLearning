using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Repository.Commands.Read
{
    public abstract record ReadCommand<T>(bool Tracked = true) where T : IEntity, new()
    {
        public required IEnumerable<Expression<Func<T, bool>>>? Filters { get; init; }
        public required IEnumerable<Expression<Func<T, object>>>? Includes { get; init; }
        public required int Size { get; init; }
        public required int ZeroStart { get; init; }
    }
}
