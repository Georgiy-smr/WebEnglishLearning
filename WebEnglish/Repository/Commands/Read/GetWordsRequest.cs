using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities;
using MediatR;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read
{
    public record GetWordsRequest() :
        ReadCommand<Word>,
        IRequest<IStatusGeneric<IEnumerable<WordDto>>>
    {
        public override string ToString()
        {
            string name = $"{nameof(GetWordsRequest)}";
            return name;
        }
    }
}
