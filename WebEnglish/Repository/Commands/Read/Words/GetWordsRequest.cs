using DataBaseOperationHelper.BaseCommandsOperations.Reading;
using Entities;
using Repository.DTO;

namespace Repository.Commands.Read.Words
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
