using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataBaseOperationHelper.BaseCommandsOperations.Reading;
using Entities;
using MediatR;
using Repository.DTO;
using StatusGeneric;

namespace Repository.Commands.Read
{
    public record GetWordsRequest() :
            ReadCommandRequest<Word,WordDto>
    {
        public override string ToString()
        {
            string name = $"{nameof(GetWordsRequest)}";
            return name;
        }
    }
}
