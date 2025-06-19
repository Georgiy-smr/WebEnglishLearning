using Repository.DTO;

namespace Repository.Commands.Update;

public record BaseUpdateCommand<T>(T Modified) : BaseCommandDataBase where T : BaseDto;