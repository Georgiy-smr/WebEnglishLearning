using MediatR;
using StatusGeneric;

namespace Repository.Commands;

/// <summary>
/// Base CRUD operation command
/// </summary>
public record BaseCommandDataBase() : IRequest<IStatusGeneric>;