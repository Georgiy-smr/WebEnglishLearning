using Repository.DTO;

namespace Repository.Commands.Create;

public abstract record BaseCreateCommand<T>(T Сreated) : BaseCommandDataBase where T : BaseDto;