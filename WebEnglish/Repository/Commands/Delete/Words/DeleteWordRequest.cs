using DataBaseOperationHelper.Abstractions;
using BaseOperation = DataBaseOperationHelper.BaseCommandsOperations.Deleting;

namespace Repository.Commands.Delete.Words;

public record DeleteWordRequest(int Id) : BaseOperation.Delete(Id)
{
    public DeleteWordRequest(BaseDto wordToRemove) : this(wordToRemove.Id) {}
    public DeleteWordRequest(IEntity entityToRemove) : this(entityToRemove.Id) { }
    public override string ToString()
    {
        string name = $"{nameof(DeleteWordRequest)} {Id}";
        return base.ToString();
    }
}