using DataBaseOperationHelper.Abstractions;
using Repository.DTO;

namespace Repository.Commands.Update;

public record OperationRequestFromDataBaseRequestUpdateCommand<T>(T Modified) : OperationRequestFromDataBase where T : BaseDto;